using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Common;

namespace VerifyIndia.Application.DTO.Registration
{
    public sealed class VerifyAadhaarOtpResponseDto
    {
        public bool Success { get; set; }

        public string FullName { get; set; }
            = string.Empty;

        public string AadhaarNumber { get; set; }
            = string.Empty;

        public string ReferenceId { get; set; }
            = string.Empty;

        public OtpPolicyDto OtpPolicy { get; set; }
            = new();
    }
}
