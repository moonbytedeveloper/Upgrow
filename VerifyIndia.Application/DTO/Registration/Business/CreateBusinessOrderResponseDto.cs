using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTO.Registration.Business
{
    public sealed class CreateBusinessOrderResponseDto
    {
        public string SessionUUID { get; set; } = string.Empty;

        public string OrderId { get; set; } = string.Empty;

        public decimal Amount { get; set; }

        public string Currency { get; set; } = "INR";

        public string RazorpayKeyId { get; set; } = string.Empty;
    }
}
