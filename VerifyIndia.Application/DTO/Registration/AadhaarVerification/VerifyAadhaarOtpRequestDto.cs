using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTO.Registration.AadhaarVerification
{
    public sealed class VerifyAadhaarOtpRequestDto
    {
        public string SessionUUID { get; set; }
            = string.Empty;

        public string Otp { get; set; }
            = string.Empty;
    }
}
