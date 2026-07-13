using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTO.Registration.VideoKyc
{
    public sealed class SubmitVideoKycResponseDto
    {
        public bool IsVerified { get; set; }

        public string Status { get; set; }
            = string.Empty;

        public string? FailureReason { get; set; }

        public string? VideoUrl { get; set; }
    }
}
