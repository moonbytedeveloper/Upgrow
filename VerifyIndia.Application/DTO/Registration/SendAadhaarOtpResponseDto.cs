using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Common;

namespace VerifyIndia.Application.DTO.Registration
{
    public sealed class SendAadhaarOtpResponseDto
    {
        public bool Success { get; set; }

        public string ClientId { get; set; }
            = string.Empty;

        public string Message { get; set; }
            = string.Empty;

        public OtpPolicyDto OtpPolicy { get; set; }
            = new();
    }
}
