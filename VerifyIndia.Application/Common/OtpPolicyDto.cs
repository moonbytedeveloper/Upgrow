using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.Common
{
    public sealed class OtpPolicyDto
    {
        public int OtpExpiresInSeconds { get; set; }

        public int MaxResendOtpCount { get; set; }

        public int ResendCooldownSeconds { get; set; }

        public int MaxVerifyAttemptCount { get; set; }

        public int LockoutDurationSeconds { get; set; }

        public int RemainingResendOtpCount { get; set; }

        public int RemainingVerifyAttemptCount { get; set; }

        public bool IsLocked { get; set; }

        public DateTimeOffset? LockedUntil { get; set; }
    }
}
