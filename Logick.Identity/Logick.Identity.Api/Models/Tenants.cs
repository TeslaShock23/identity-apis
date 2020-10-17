using System.Collections.Generic;

namespace Logick.Identity.Api.Models
{
    public class Tenants : List<Tenant>
    {
        public Tenants()
        {
            Add(new Tenant{ TenantId = "asdf", SecretKey = "asdf", ApiKey = "postman"});         
            Add(new Tenant{ TenantId = "asdf", SecretKey = "asdf", ApiKey = "Logick.Test.Api"});
        }
    }
}