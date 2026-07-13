using AuthenticateIndia.Shared.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Constant;
using VerifyIndia.Application.DTO.Notification;
using VerifyIndia.Application.DTO.Registration.EmailVerification;
using VerifyIndia.Application.Interfaces.Notification;
using VerifyIndia.Application.IServices;
using VerifyIndia.Application.IServices.Master;
using VerifyIndia.Application.IServices.Registration;
using VerifyIndia.Domain.Entities.Registration;
using VerifyIndia.Domain.Enums;
using VerifyIndia.Domain.IRepositories.Registration;

namespace VerifyIndia.Application.Services.Registration
{
    public class EmailVerificationService
        : IEmailVerificationService
    {
        private readonly IMasterCustomerService _customerService;
        private readonly ICustomerEmailVerificationRepository _customerEmailVerificationRepository;
        private readonly INotificationOrchestrator _notificationOrchestrator;
        private readonly IAppSettingService _appSettingService;

        public EmailVerificationService(
            IMasterCustomerService customerService,
            ICustomerEmailVerificationRepository customerEmailVerificationRepository,
            INotificationOrchestrator notificationOrchestrator,
            IAppSettingService appSettingService)
        {
            _customerService =
                customerService;

            _customerEmailVerificationRepository =
                customerEmailVerificationRepository;

            _notificationOrchestrator =
                notificationOrchestrator;

            _appSettingService =
                appSettingService;
        }

        public async Task<SendEmailOtpResponseDto>
            SendOtpAsync(
                string customerUuid,
                SendEmailOtpRequestDto request)
        {
            var customer =
                await _customerService
                    .GetCustomerByUUID(
                        customerUuid);

            if (customer == null)
            {
                throw new Exception(
                    "Customer not found.");
            }

            var email =
                request.Email?.Trim();

            if (string.IsNullOrWhiteSpace(
                    email))
            {
                throw new Exception(
                    "Email address is required.");
            }

            var latest =
                await _customerEmailVerificationRepository
                    .GetLatestAsync(
                        customerUuid);

            if (latest != null
                &&
                latest.LastResendAt.HasValue)
            {
                var cooldownSeconds =
                    await _appSettingService
                        .GetIntValueAsync(
                            AppSettingKeys
                                .OTP_EMAIL_RESEND_COOLDOWN_SECONDS,
                            30);

                if (latest.LastResendAt.Value
                        .AddSeconds(
                            cooldownSeconds)
                    >
                    DateTimeOffset.UtcNow)
                {
                    throw new Exception(
                        "Please wait before requesting another OTP.");
                }
            }

            await _customerEmailVerificationRepository
                .DeactivateActiveAsync(
                    customerUuid);

            var expirySeconds =
                await _appSettingService
                    .GetIntValueAsync(
                        AppSettingKeys
                            .OTP_EMAIL_EXPIRY_SECONDS,
                        300);

            var expiryMinutes = expirySeconds / 60;

            var otp = Utils.GenerateOtp(4);

            var now =
                DateTimeOffset.UtcNow;

            var verification =
                new CustomerEmailVerification
                {
                    UUID =
                        Utils.GetUUID(),

                    CustomerUUID =
                        customerUuid,

                    Email =
                        email,

                    OTP =
                        Utils.GenerateHash(otp),

                    OTPSentAt =
                        now,

                    OTPExpiresAt =
                        now.AddSeconds(
                            expirySeconds),

                    LastResendAt =
                        now,

                    IsVerified =
                        false,

                    IsActive =
                        true,

                    CreatedAt =
                        now
                };

            await _customerEmailVerificationRepository
                .AddAsync(
                    verification);

            try
            {
                await _notificationOrchestrator
                    .SendAsync(
                        new NotificationRequestDto
                        {
                            EventCode =
                                NotificationEvents.EmailVerification,

                            UserId =
                                customerUuid,
                            Email =
                                request.Email,

                            Variables =
                                new Dictionary<string, object>
                                {
                                    ["user_name"] =
                                        $"{customer.FName} {customer.LName}".Trim(),

                                    ["otp_code"] =
                                        otp,

                                    ["expiry_time"] =
                                        expiryMinutes + " Minutes"
                                },

                            Channels =
                            [
                                NotificationChannel.Email
                            ]
                        });

                return new SendEmailOtpResponseDto
                {
                    OTPExpiresAtUtc =
                        verification.OTPExpiresAt
                };
            }
            catch (Exception ex)
            {
                verification.IsActive =
                    false;

                verification.FailureReason =
                    ex.Message;

                verification.UpdatedAt =
                    DateTimeOffset.UtcNow;

                await _customerEmailVerificationRepository
                    .UpdateAsync(
                        verification);

                throw new Exception(
                    "Failed to send verification email.");
            }
        }

        public async Task<VerifyEmailOtpResponseDto> VerifyOtpAsync(
            string customerUuid,
            VerifyEmailOtpRequestDto request)
        {
            var customer =
                await _customerService
                    .GetCustomerByUUID(
                        customerUuid);

            if (customer == null)
            {
                throw new Exception(
                    "Customer not found.");
            }

            var verification =
                await _customerEmailVerificationRepository
                    .GetLatestAsync(
                        customerUuid);

            if (verification == null
                ||
                !verification.IsActive)
            {
                throw new Exception(
                    "Email verification request not found.");
            }

            if (verification.IsVerified)
            {
                throw new Exception(
                    "Email is already verified.");
            }

            if (verification.OTPExpiresAt <=
                DateTimeOffset.UtcNow)
            {
                verification.IsActive =
                    false;

                verification.FailureReason =
                    "OTP expired.";

                verification.UpdatedAt =
                    DateTimeOffset.UtcNow;

                await _customerEmailVerificationRepository
                    .UpdateAsync(
                        verification);

                throw new Exception(
                    "OTP has expired.");
            }

            if (!string.Equals(
                    verification.OTP,
                    Utils.GenerateHash(request.OTP?.Trim()),
                    StringComparison.Ordinal))
            {
                verification.FailureReason =
                    "Invalid OTP.";

                verification.UpdatedAt =
                    DateTimeOffset.UtcNow;

                await _customerEmailVerificationRepository
                    .UpdateAsync(
                        verification);

                throw new Exception(
                    "Invalid OTP.");
            }

            verification.IsVerified =
                true;

            verification.VerifiedAt =
                DateTimeOffset.UtcNow;

            verification.UpdatedAt =
                DateTimeOffset.UtcNow;

            await _customerEmailVerificationRepository
                .UpdateAsync(
                    verification);

            customer.Email =
                verification.Email;

            await _customerService
                .UpdateCustomerAsync(
                    customer, true);

            return new VerifyEmailOtpResponseDto
            {
                IsVerified =
                    true
            };
        }
    }
}
