using System.Collections.Generic;

namespace Logick.Identity.Api.Models
{
    public class ClaimModel
    {
        public string Email { get; set; }
        public List<ClaimPair> Claims { get; set; }
    }
}