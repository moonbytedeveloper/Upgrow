using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTO.Transaction
{
    public class SendConsentLinkResponseDto
    {
        public string TransactionUUID { get; set; }
            = string.Empty;

        public string ConsentMobileNo { get; set; }
            = string.Empty;

        public bool IsConsentSent { get; set; }

        public DateTimeOffset ConsentLinkSentAt
        { get; set; }
    }
}
