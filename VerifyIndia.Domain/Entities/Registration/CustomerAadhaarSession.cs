namespace VerifyIndia.Domain.Entities.Registration
{
    public class CustomerAadhaarSession
    {
        public decimal Id { get; set; }

        public string UUID { get; set; }
            = string.Empty;

        public string CustomerUUID { get; set; }
            = string.Empty;

        public string? AadhaarNumberEncrypted { get; set; }

        public string? ClientId { get; set; }

        public string? RefId { get; set; }

        public string? RazorpayOrderId { get; set; }

        public string? RazorpayPaymentId { get; set; }

        public decimal Amount { get; set; }

        public bool IsPaymentCompleted { get; set; }

        public bool IsOtpSent { get; set; }

        public bool IsVerified { get; set; }

        public int VerifyAttemptCount { get; set; }

        public int OtpResendCount { get; set; }

        public DateTimeOffset? LastResendAt { get; set; }

        public DateTimeOffset? LockedUntil { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset? UpdatedAt { get; set; }

        public DateTimeOffset? OtpSentAt { get; set; }

        public DateTimeOffset? VerifiedAt { get; set; }

        public string? FailureReason { get; set; }

        public bool IsActive { get; set; }

        public byte[]? RecordHash { get; set; }
    }
}