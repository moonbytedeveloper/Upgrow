using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Domain.Models
{
    public sealed class VerificationResult
    {
        public bool IsSuccess { get; init; }

        public string? DocumentBase64 { get; init; }

        /// <summary>
        /// Indicates whether another provider step
        /// is required.
        /// </summary>
        public bool RequiresUserInput { get; init; }

        /// <summary>
        /// OTP / AUTH_CODE / FACE etc.
        /// </summary>
        public string? InputType { get; init; }

        /// <summary>
        /// Next verification step.
        /// Example:
        /// AADHAAR_VERIFY_OTP
        /// </summary>
        public string? NextVerificationCode { get; init; }
    }
}
