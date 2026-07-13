using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.Verification.Documents
{
    public sealed class TransactionExecutionResult
    {
        public string TransactionUUID { get; init; }
            = string.Empty;

        public string ProcessingStatus { get; init; }
            = string.Empty;

        public IReadOnlyList<TransactionExecutionDetailResult>
            Details
        { get; init; }
            = [];
    }

    public sealed class TransactionExecutionDetailResult
    {
        public string TransactionDetailUuid { get; init; }
            = string.Empty;

        public string CurrentVerificationCode { get; init; }
            = string.Empty;

        public bool IsSuccess { get; init; }

        //public object? Response { get; init; }

        public string? DocumentBase64 { get; init; }

        public string? Message { get; init; }

        public bool RequiresUserInput { get; init; }

        public string? NextVerificationCode { get; init; }

        // Returned only when RequiresUserInput == true
        public string? ClientId { get; set; }

        // Frontend can show Resend OTP button
        public bool CanResend { get; set; }

        // In seconds
        public int? OtpExpiryInSeconds { get; set; }
        public int? ResendOtpCooldownInSeconds { get; set; } = 60;
    }
}
