using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTO.Registration
{
    public sealed class VerifyOtpResultDto
    {
        public bool Success { get; set; }

        public string Message { get; set; }
            = string.Empty;

        public VerifyOtpResponseDto Response { get; set; }
            = new();
    }
}
