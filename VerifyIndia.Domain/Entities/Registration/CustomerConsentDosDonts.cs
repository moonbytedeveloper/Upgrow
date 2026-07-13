using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Domain.Entities.Registration
{
    public class CustomerConsentDosDonts
    {
        public decimal Id { get; set; }

        public string UUID { get; set; }
            = string.Empty;

        public string CustomerUUID { get; set; }
            = string.Empty;

        public string DocumentUUID { get; set; }
            = string.Empty;

        public bool ConsentGiven { get; set; }

        public DateTimeOffset? ConsentGivenAt { get; set; }

        public string? IPAddress { get; set; }

        public string? Platform { get; set; }

        public bool IsActive { get; set; }

        public byte[]? RecordHash { get; set; }
    }
}
