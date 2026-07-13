using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Domain.Entities.Registration
{
    public class CustomerEmailVerification
        : BaseEntity
    {
        public string CustomerUUID { get; set; }
            = string.Empty;

        public string Email { get; set; }
            = string.Empty;

        public string OTP { get; set; }
            = string.Empty;

        public DateTimeOffset OTPSentAt { get; set; }

        public DateTimeOffset OTPExpiresAt { get; set; }

        public DateTimeOffset? LastResendAt { get; set; }

        public bool IsVerified { get; set; }

        public DateTimeOffset? VerifiedAt { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }

        public string? FailureReason { get; set; }
    }
}
