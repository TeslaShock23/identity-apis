using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Logick.Identity.Api.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Logick.Identity.Api.JwtToken
{
    public class JwtTokenBuilder : IJwtTokenBuilder
    {
        private readonly JwtTokenOptions _jwtOptions;

        public JwtTokenBuilder(IOptions<JwtTokenOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value;
        }

        public string BuildJwtToken(IEnumerable<Claim> claims, Tenant tenant)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var issuer = _jwtOptions.Issuer;
            var audience = tenant.ApiKey;
            var validity = DateTime.UtcNow.AddMinutes(_jwtOptions.TokenExpiry);
            
            var token = new JwtSecurityToken(issuer, audience, claims, expires: validity, signingCredentials: credentials);
            
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}