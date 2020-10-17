namespace Logick.Identity.Api.Models
{
    public class Tenant
    {
        public string TenantId { get; set; }
        public string SecretKey { get; set; }
        public string ApiKey { get; set; }
    }
}