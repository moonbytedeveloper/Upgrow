using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Domain.Entities
{
    public class TransactionDetail : BaseEntity
    {
        public string TransactionUUID { get; set; }
            = string.Empty;

        public string ApiUUID { get; set; }
            = string.Empty;

        public string? ReqPl { get; set; }
        public string? VerificationCode { get; set; }
        public string CurrentVerificationCode { get; set; } = string.Empty;

        public string PricingUUID { get; set; }
            = string.Empty;

        public decimal ApiCharge { get; set; }

        public DateTimeOffset RecordDatetime { get; set; }

        public bool IsMultistageRoute { get; set; }

        public bool IsConsentRequired { get; set; }

        /// <summary>
        /// Pending / WaitingForCustomerInput /
        /// Completed / Failed
        /// </summary>
        public string ProcessingStatus { get; set; }

        public virtual Transaction
            Transaction
        { get; set; }
            = null!;
    }
}
