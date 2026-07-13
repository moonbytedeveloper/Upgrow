using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.Transaction
{
    public class CreatePaymentOrderResponseDto
    {
        public string TransactionUUID { get; set; }
            = string.Empty;

        public string OrderId { get; set; }
            = string.Empty;

        public string KeyId { get; set; }
            = string.Empty;

        public decimal Amount { get; set; }
    }
}
