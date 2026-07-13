using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTO.Registration.EmailVerification
{
    public sealed class SendEmailOtpRequestDto
    {
        public string Email { get; set; }
            = string.Empty;
    }
}
