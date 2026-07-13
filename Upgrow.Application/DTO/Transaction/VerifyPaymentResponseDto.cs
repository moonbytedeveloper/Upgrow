using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.Transaction
{
    public class VerifyPaymentResponseDto
    {
        public string TransactionUUID { get; set; }
            = string.Empty;

        public string PaymentStatus { get; set; }
            = string.Empty;

        public bool IsPaymentSuccess { get; set; }
    }
}
