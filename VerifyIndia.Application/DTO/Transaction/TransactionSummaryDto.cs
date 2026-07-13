using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTO.Transaction
{
    public class TransactionSummaryDto
    {
        public string TransactionUUID { get; set; }
            = string.Empty;

        public string TransactionNo { get; set; }
            = string.Empty;

        public string PaymentStatus { get; set; }
            = string.Empty;

        public string PaymentMode { get; set; }
            = string.Empty;

        public decimal TotalPayableAmount { get; set; }

        public string ConsentStatus { get; set; }
            = string.Empty;

        public bool IsConsentSentOnMobile
        { get; set; }
    }
}
