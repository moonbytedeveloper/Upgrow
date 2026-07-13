using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Domain.Entities.Registration
{
    public class CustomerVideoKyc : BaseEntity
    {
        public string CustomerUUID { get; set; }
            = string.Empty;

        public string? VideoUrl { get; set; }

        public string? ChallengeText { get; set; }

        public DateTimeOffset? ChallengeCreatedAt { get; set; }

        public DateTimeOffset? ChallengeExpiresAt { get; set; }

        public string? SpokenText { get; set; }

        public decimal? SpeechMatchPercentage { get; set; }

        public decimal? FaceDetectionPercentage { get; set; }

        public bool FaceDetected { get; set; }

        public bool IsVerified { get; set; }

        public string? FailureReason { get; set; }

        public DateTimeOffset VerificationTimeStamp { get; set; }
    }
}
