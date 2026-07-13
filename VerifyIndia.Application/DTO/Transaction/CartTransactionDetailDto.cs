using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.Transaction
{
    public class CartTransactionDetailDto
    {
        public string ApiUUID { get; set; }
            = string.Empty;

        public string VerificationCode { get; set; }
            = string.Empty;

        public string PricingUUID { get; set; }
            = string.Empty;

        public decimal ApiCharge { get; set; }

        public string ReqPayload { get; set; }
            = string.Empty;

        public bool IsMultipleEndPoint { get; set; }

        public bool IsConsentBased { get; set; }
    }
}
