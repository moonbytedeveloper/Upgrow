using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.Registration.VideoKyc
{
    public sealed class UploadVideoKycResponseDto
    {
        public bool IsVerified { get; set; }

        public string? FailureReason { get; set; }
    }
}
