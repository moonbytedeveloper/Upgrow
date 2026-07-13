using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTO.Customer
{
    public class CustomerConsentDto
    {
        public string CustomerUUID { get; set; } = string.Empty;
        public string? PolicyUUID { get; set; }        
        public string? PolicyContentHash { get; set; }
        public string? PolicyVersion { get; set; }
        public bool ConsentGiven { get; set; }
        public DateTimeOffset? SignedAt { get; set; }
        public string? IpAddress { get; set; }
        public string? Platform { get; set; }
        public string? SignId { get; set; }
        public string? LatLong { get; set; } 
    }
}
