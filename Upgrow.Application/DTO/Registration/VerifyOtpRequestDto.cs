using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.Registration
{
    public sealed class VerifyOtpRequestDto
    {
        public string MobileNumber { get; set; } = string.Empty;

        public string Otp { get; set; } = string.Empty;
    }
}
