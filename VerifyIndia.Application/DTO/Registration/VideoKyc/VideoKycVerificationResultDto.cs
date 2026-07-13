using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTO.Registration.VideoKyc
{
    public sealed class VideoKycVerificationResultDto
    {
        public bool IsMatched { get; set; }

        public bool FaceDetected { get; set; }

        public decimal FaceDetectionPercentage { get; set; }

        public string SpokenText { get; set; } = string.Empty;

        public decimal SpeechMatchPercentage { get; set; }

        public string? FailureReason { get; set; }
    }
}
