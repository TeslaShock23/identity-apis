namespace Logick.Test.Api.Models
{
    public class JwtTokenOptions
    {
        public string SecretKey { get; set; }
        
        public string Issuer { get; set; }

        public string Audience { get; set; }

        public double TokenExpiry { get; set; }
    }
}