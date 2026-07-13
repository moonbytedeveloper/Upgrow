using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Domain.Entities
{
    public class Transaction : BaseEntity
    {
        public string TransactionNo { get; set; }
            = string.Empty;

        public string CartUUID { get; set; }
            = string.Empty;

        public string AuthFor { get; set; }
            = string.Empty;

        public decimal BaseCreditsTotal { get; set; }

        public decimal ConsentCreditsTotal { get; set; }

        public decimal PayableBaseAmount { get; set; }

        public decimal IGST { get; set; }

        public decimal CGST { get; set; }

        public decimal SGST { get; set; }

        public decimal GST { get; set; }

        public decimal TotalPayableAmount { get; set; }

        public string PaymentMode { get; set; }
            = string.Empty;

        public string? PaymentFrom { get; set; }

        public string VerifierUUID { get; set; }
            = string.Empty;

        public string PaymentStatus { get; set; }
            = string.Empty;

        public string TransactionStatus { get; set; }
            = string.Empty;

        public string ProcessingStatus { get; set; } 

        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? CompletedAt { get; set; }

        public virtual ICollection<TransactionDetail>
            TransactionDetails
        { get; set; }
            = new List<TransactionDetail>();

        public virtual TransactionConsent?
            TransactionConsent
        { get; set; }
    }
}
