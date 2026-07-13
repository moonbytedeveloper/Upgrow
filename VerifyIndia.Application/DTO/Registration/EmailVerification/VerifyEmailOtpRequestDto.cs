using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.Registration.EmailVerification
{
    public sealed class VerifyEmailOtpRequestDto
    {
        public string OTP { get; set; }
            = string.Empty;
    }
}
