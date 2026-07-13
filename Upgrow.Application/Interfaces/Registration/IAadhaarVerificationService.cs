using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.DTO.Registration;

namespace Upgrow.Application.Interfaces.Registration
{
    public interface IAadhaarVerificationService
    {
        Task<SendAadhaarOtpResponseDto>
            SendOtpAsync(
                string aadhaarNumber);

        Task<VerifyAadhaarOtpResponseDto>
            VerifyOtpAsync(
                string refId,
                string clientId,
                string otp);
    }
}
