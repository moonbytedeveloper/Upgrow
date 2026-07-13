using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTO.Payment
{
    public sealed class RazorpayOrderResponseDto
    {
        public string OrderId { get; set; }
            = string.Empty;

        public decimal Amount { get; set; }

        public string KeyId { get; set; }
            = string.Empty;
    }
}
