using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Logick.Identity.Api.JwtToken;
using Logick.Identity.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Logick.Identity.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly JwtTokenOptions _jwtOptions;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly Tenants _tenants;
        private readonly IJwtTokenBuilder _tokenBuilder;
        private readonly UserManager<IdentityUser> _userManager;

        public AuthController(IJwtTokenBuilder tokenBuilder,
            IOptions<JwtTokenOptions> jwtOptions,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            Tenants tenants)
        {
            _tokenBuilder = tokenBuilder;
            _jwtOptions = jwtOptions.Value;
            _userManager = userManager;
            _signInManager = signInManager;
            _tenants = tenants;
        }

        [HttpPost]
        public async Task<IActionResult> RequestToken([FromBody] UserCredentials credentials)
        {
            // Determine Api Key Exists
            Tenant tenant = null;
            var requestApiKey = Request.Headers["x-api-key"].FirstOrDefault();
            if (!string.IsNullOrEmpty(requestApiKey)) tenant = _tenants.FirstOrDefault(t => string.Equals(t.ApiKey, requestApiKey, StringComparison.InvariantCultureIgnoreCase));

            if (tenant is null) return Unauthorized("Tenant doesn't exist");

            // Validate User Login
            var user = await _userManager.FindByEmailAsync(credentials.Email);
            if (user is null) return BadRequest("User doesn't exist");

            var signInResult = await _signInManager.PasswordSignInAsync(user, credentials.Password, false, false);
            if (!signInResult.Succeeded) return BadRequest("Wrong password");

            // Create Token
            var claims = await _userManager.GetClaimsAsync(user);
            var token = _tokenBuilder.BuildJwtToken(claims, tenant);
            return Ok(new
            {
                AccessToken = token,
                Expiry = DateTime.UtcNow.AddMinutes(_jwtOptions.TokenExpiry)
            });
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateUser([FromBody] UserCredentials credentials)
        {
            var result = await _userManager.CreateAsync(new IdentityUser(credentials.Email) {Email = credentials.Email}, credentials.Password);
            if (result.Succeeded) return Ok();

            return BadRequest(result.Errors);
        }

        [HttpPost]
        [Route("claims")]
        public async Task<IActionResult> AddClaim([FromBody] ClaimModel claimModel)
        {
            var claims = claimModel.Claims.Select(claim => new Claim(claim.Type, claim.Value)).ToList();

            var user = await _userManager.FindByEmailAsync(claimModel.Email);
            var result = await _userManager.AddClaimsAsync(user, claims);

            if (result.Succeeded) return Ok();

            return BadRequest(result.Errors);
        }
        
        [HttpPost]
        [Route("remove")]
        public async Task<IActionResult> DeleteUser([FromBody] UserCredentials credentials)
        {
            var user = await _userManager.FindByNameAsync(credentials.Email);
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded) return Ok();

            return BadRequest(result.Errors);
        }
    }
}