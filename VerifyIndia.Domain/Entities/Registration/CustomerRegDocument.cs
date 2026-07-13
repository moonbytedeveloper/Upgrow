using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Domain.Entities.Registration
{
    public class CustomerRegDocument
    {
        public decimal Id { get; set; }

        public string UUID { get; set; }
            = string.Empty;

        public string CustomerUUID { get; set; }
            = string.Empty;

        public string? RecordType { get; set; }

        public string? RecordNo { get; set; }

        public string? RecordCategory { get; set; }

        public bool IsVerified { get; set; }

        public DateTimeOffset? VerificationTimeStamp { get; set; }

        public string? VerifiedFrom { get; set; }

        public string? IpAddress { get; set; }

        public string? Latitude { get; set; }

        public string? Longitude { get; set; }

        public string? City { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        public byte[]? RecordHash { get; set; }
    }
}
