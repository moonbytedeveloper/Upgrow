using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Domain.Entities.Registration
{
    public class CustomerOtp : BaseEntity
    {
        public string CustomerUUID { get; set; }
            = string.Empty;

        public string OtpHash { get; set; }
            = string.Empty;

        public DateTime ExpiresAtUtc { get; set; }

        public int VerifyAttemptCount { get; set; }

        public int ResendCount { get; set; }

        public DateTimeOffset? LastResendAt { get; set; }

        public DateTimeOffset? LockedUntil { get; set; }

        public bool IsUsed { get; set; }

        public DateTime CreatedAtUtc { get; set; }
    }
}
