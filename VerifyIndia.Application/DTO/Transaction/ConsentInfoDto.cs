using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTO.Transaction
{
    public class ConsentInfoDto
    {
        public string ConsentStatus { get; set; }
            = string.Empty;

        public bool IsConsentSentOnMobile { get; set; }

        public DateTimeOffset? ConsentLinkSentAt { get; set; }

        public DateTimeOffset? ConsentExpiresAt { get; set; }
    }
}
