using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Logick.Identity.Api
{
    public class IdentityContext : IdentityDbContext
    {
        public IdentityContext(DbContextOptions options) : base(options)
        {
        }
    }
}