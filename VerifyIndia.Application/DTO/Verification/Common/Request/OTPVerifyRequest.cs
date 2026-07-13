using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTO.Verification.Common.Request
{
    public class OTPVerifyRequest
    {
        public string? client_id { get; set; }
        public string? otp { get; set; }
    }
}
