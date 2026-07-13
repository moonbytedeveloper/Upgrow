using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTO.Registration.EmailVerification
{
    public sealed class SendEmailOtpResponseDto
    {
        public DateTimeOffset OTPExpiresAtUtc
        {
            get;
            set;
        }
    }
}
