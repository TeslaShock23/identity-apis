namespace Logick.Identity.Api.JwtToken
{
    public class JwtTokenOptions
    {
        public string SecretKey { get; set; }
        
        public string Issuer { get; set; }

        public double TokenExpiry { get; set; }
    }
}