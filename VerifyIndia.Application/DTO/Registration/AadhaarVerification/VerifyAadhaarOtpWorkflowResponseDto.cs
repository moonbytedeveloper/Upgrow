using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Common;

namespace VerifyIndia.Application.DTO.Registration.AadhaarVerification
{
    public sealed class VerifyAadhaarOtpWorkflowResponseDto
    {
        public bool Success { get; set; }

        public string Message { get; set; }
            = string.Empty;

        public RegistrationStateDto RegistrationState { get; set; }
            = new();

        public OtpPolicyDto OtpPolicy { get; set; }
            = new();
    }
}
