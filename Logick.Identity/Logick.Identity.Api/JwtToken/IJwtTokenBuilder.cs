using System.Collections.Generic;
using System.Security.Claims;
using Logick.Identity.Api.Models;

namespace Logick.Identity.Api.JwtToken
{
    public interface IJwtTokenBuilder
    {
        string BuildJwtToken(IEnumerable<Claim> claims, Tenant tenant);
    }
}