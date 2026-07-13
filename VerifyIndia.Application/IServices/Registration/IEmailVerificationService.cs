using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.DTO.Registration.EmailVerification;

namespace VerifyIndia.Application.IServices.Registration
{
    public interface IEmailVerificationService
    {
        Task<SendEmailOtpResponseDto>
            SendOtpAsync(
                string customerUuid,
                SendEmailOtpRequestDto request);

        Task<VerifyEmailOtpResponseDto>
            VerifyOtpAsync(
                string customerUuid,
                VerifyEmailOtpRequestDto request);
    }
}
