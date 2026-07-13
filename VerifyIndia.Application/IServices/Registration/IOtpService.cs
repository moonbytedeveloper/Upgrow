using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.DTO.Registration;
using VerifyIndia.Domain.Enums;

namespace VerifyIndia.Application.IServices.Registration
{
    public interface IOtpService
    {
        Task SendOtpAsync(
    string customerUuid,
    string mobileNumber,
    NotificationChannel channel,
    int resendCount,
    int verifyAttemptCount);

        Task<bool> VerifyOtpAsync(
            string customerUuid,
            string otp);
    }
}
