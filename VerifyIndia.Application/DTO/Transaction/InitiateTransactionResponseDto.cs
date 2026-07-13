using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.Transaction
{
    public class InitiateTransactionResponseDto
    {
        public string TransactionUUID { get; set; }
            = string.Empty;

        public string TransactionNo { get; set; }
            = string.Empty;

        public string PaymentMode { get; set; }
            = string.Empty;

        public string PaymentStatus { get; set; }
            = string.Empty;

        public decimal BaseCreditsTotal { get; set; }

        public decimal ConsentCreditsTotal { get; set; }

        public decimal PayableBaseAmount { get; set; }

        public decimal GST { get; set; }

        public decimal TotalPayableAmount { get; set; }

        public bool ConsentRequired { get; set; }

        public bool PaymentRequired { get; set; }
    }
}
