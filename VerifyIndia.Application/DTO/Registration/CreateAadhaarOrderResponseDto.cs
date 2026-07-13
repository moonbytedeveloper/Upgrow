using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTO.Registration
{
    public sealed class CreateAadhaarOrderResponseDto
    {
        public string SessionUUID { get; set; }
            = string.Empty;

        public string OrderId { get; set; }
            = string.Empty;

        public decimal Amount { get; set; }

        public string RazorpayKeyId { get; set; }
            = string.Empty;
    }
}
