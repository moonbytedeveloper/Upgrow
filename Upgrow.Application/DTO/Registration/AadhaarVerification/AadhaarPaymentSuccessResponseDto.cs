using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Common;

namespace Upgrow.Application.DTO.Registration.AadhaarVerification
{
    public sealed class AadhaarPaymentSuccessResponseDto
    {
        public string SessionUUID { get; set; }
            = string.Empty;

        public string ClientId { get; set; }
            = string.Empty;

        public string RefId { get; set; }
            = string.Empty;

        public bool OtpSent { get; set; }

        public OtpPolicyDto OtpPolicy { get; set; }
            = new();
    }
}
