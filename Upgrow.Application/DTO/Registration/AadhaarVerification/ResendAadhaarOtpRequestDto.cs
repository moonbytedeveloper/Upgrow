using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.Registration.AadhaarVerification
{
    public sealed class ResendAadhaarOtpRequestDto
    {

        public string AadhaarNumber { get; set; }
            = string.Empty;
    }
}
