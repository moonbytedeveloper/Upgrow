using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Verification.Interfaces;
using static VerifyIndia.Application.Constants;

namespace VerifyIndia.Application.Services.TransactionProcessor
{
    public sealed class VerificationStepResolver
    : IVerificationStepResolver
    {
        public string GetPreviousVerificationCode(
            string currentVerificationCode)
        {
            return currentVerificationCode switch
            {
                VerificationCodes.AadhaarVerifyOtp =>
                    VerificationCodes.AadhaarSendOTP,

                _ =>
                    throw new NotSupportedException(
                        $"Restart is not supported for verification '{currentVerificationCode}'.")
            };
        }
    }
}
