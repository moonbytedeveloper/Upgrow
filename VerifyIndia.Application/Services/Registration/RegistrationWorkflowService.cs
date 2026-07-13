using AuthenticateIndia.Shared.Constants;
using AuthenticateIndia.Shared.Constants.Registration;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using VerifyIndia.Application.Common;
using VerifyIndia.Application.Common.Results;
using VerifyIndia.Application.Constant;
using VerifyIndia.Application.DTO.Registration;
using VerifyIndia.Application.DTO.Registration.AadhaarVerification;
using VerifyIndia.Application.DTO.Registration.VideoKyc;
using VerifyIndia.Application.Interfaces.Registration;
using VerifyIndia.Application.IServices;
using VerifyIndia.Application.IServices.Auth;
using VerifyIndia.Application.IServices.Master;
using VerifyIndia.Application.IServices.Payment;
using VerifyIndia.Application.IServices.Registration;
using VerifyIndia.Domain.Entities;
using VerifyIndia.Domain.Entities.Registration;
using VerifyIndia.Domain.IRepositories.Master;
using VerifyIndia.Domain.IRepositories.Registration;
using ICustomerVideoKycRepository = VerifyIndia.Application.Interfaces.Registration.ICustomerVideoKycRepository;
using RegistrationSteps = AuthenticateIndia.Shared.Constants.Registration.RegistrationSteps;

namespace VerifyIndia.Application.Services.Registration
{
    public class RegistrationWorkflowService: IRegistrationWorkflowService
    {
        private readonly ICustomerConsentDosDontsRepository
            _customerConsentDosDontsRepository;

        private readonly IMasterDosDontsDocumentRepository
            _masterDosDontsDocumentRepository;

        private readonly IHttpContextAccessor
            _httpContextAccessor;

        private readonly IVideoKycVerificationService _videoKycVerificationService;
        private readonly ICustomerOtpRepository _otpRepository;
        private readonly IAppSettingService _appSettingService;
        private readonly AuditContext _auditContext;
        private readonly IFileUploadService _fileUploadService;
        private readonly ICustomerVideoKycRepository _customerVideoKycRepository;
        private readonly HttpClient _httpClient;
        private readonly IMasterStateService _stateService;
        private readonly IMasterCityService _cityService;
        private readonly IMasterPolicyService _policyService;
        private readonly IEncryptionService _encryptionService;
        private readonly IAadhaarVerificationService _aadhaarVerificationService;
        private readonly ICustomerRegDocumentRepository _customerRegDocumentRepository;
        private readonly ICustomerAadhaarSessionRepository _aadhaarSessionRepository;

        private readonly IVerificationFeeRepository _verificationFeeRepository;

        private readonly IRazorpayService _razorpayService;
        private readonly IOtpService _otpService;

        private readonly IJwtTokenService _jwtTokenService;

        private readonly IMasterCustomerService _customerService;

        private readonly IRegistrationStateBuilder _stateBuilder;
        private readonly ICustomerConsentRepository _customerConsentRepository;
        public RegistrationWorkflowService(
    IMasterDosDontsDocumentRepository masterDosDontsDocumentRepository,
    IVideoKycVerificationService videoKycVerificationService,
    ICustomerOtpRepository otpRepository,
    IAppSettingService appSettingService,
    AuditContext auditContext,
    IFileUploadService fileUploadService,
    ICustomerVideoKycRepository customerVideoKycRepository,
    HttpClient httpClient,
    IMasterStateService stateService,
    IMasterCityService cityService,
    IMasterPolicyService policyService,
    IEncryptionService encryptionService,
    IRazorpayService razorpayService,
    ICustomerAadhaarSessionRepository aadhaarSessionRepository,
    IVerificationFeeRepository verificationFeeRepository,
    IAadhaarVerificationService aadhaarVerificationService,
    ICustomerRegDocumentRepository customerRegDocumentRepository,
    ICustomerConsentRepository customerConsentRepository,
    ICustomerConsentDosDontsRepository customerConsentDosDontsRepository,
    IOtpService otpService,
    IJwtTokenService jwtTokenService,
    IMasterCustomerService customerService,
    IRegistrationStateBuilder stateBuilder)
        {
            _masterDosDontsDocumentRepository = masterDosDontsDocumentRepository;
            _videoKycVerificationService = videoKycVerificationService;
            _otpRepository = otpRepository;
            _appSettingService = appSettingService;
            _auditContext = auditContext;
            _fileUploadService = fileUploadService;
            _customerVideoKycRepository = customerVideoKycRepository;
            _httpClient = httpClient;
            _stateService = stateService;
            _cityService = cityService;
            _policyService = policyService;
            _encryptionService = encryptionService;
            _razorpayService = razorpayService;
            _aadhaarSessionRepository = aadhaarSessionRepository;
            _verificationFeeRepository = verificationFeeRepository;
            _aadhaarVerificationService = aadhaarVerificationService;
            _otpService = otpService;
            _jwtTokenService = jwtTokenService;
            _customerService = customerService;
            _stateBuilder = stateBuilder;
            _customerConsentRepository = customerConsentRepository;
            _customerConsentDosDontsRepository = customerConsentDosDontsRepository;
            _customerRegDocumentRepository = customerRegDocumentRepository;
        }

        #region RazorPay Payment for Aadhaar Verification
        public async Task<CreateAadhaarOrderResponseDto> CreateAadhaarOrderAsync(
            string customerUuid,
            VerifyAadhaarRequestDto request)
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

            if (customer.ACType  == AccountType.ORGANIZATION &&
                customer.CurrentStep != RegistrationSteps.VERIFY_AADHAAR)
            {
                throw new Exception(
                    "Organization verification is pending.");
            }

            var aadhaarHash = Utils.GenerateHash(request.AadhaarNumber);

            var existingCustomer =
                await _customerService
                    .GetByAadhaarHashAsync(
                        aadhaarHash);

            if (existingCustomer != null
                &&
                existingCustomer.UUID != customerUuid)
            {
                throw new Exception(
                    "This Aadhaar number is already linked to another account.");
            }

            //
            // First Aadhaar attempt
            //
            if (customer.CurrentStep ==
                RegistrationSteps.SELECT_ACCOUNT_TYPE)
            {
                if (string.IsNullOrWhiteSpace(
                        request.AccountType))
                {
                    throw new Exception(
                        "Account type is required.");
                }

                customer.ACType = request.AccountType;

                if (request.AccountType == AccountType.ORGANIZATION)
                {
                    customer.CurrentStep =
                        RegistrationSteps.BusinessType;
                }
                /* else
                {
                    customer.CurrentStep =
                        RegistrationSteps.VERIFY_AADHAAR;
                }*/

                if (customer.CurrentStep == RegistrationSteps.BusinessType)
                {
                    throw new Exception("Invalid registration step.");
                }

                await _customerService
                    .UpdateCustomerAsync(
                        customer);
            }

            ValidateCurrentStep(
                customer.CurrentStep,
                RegistrationSteps.SELECT_ACCOUNT_TYPE);

            var fee = await _verificationFeeRepository.GetByTypeAsync(VerificationFeeConstants.AADHAAR);

            if (fee == null)
            {
                throw new Exception(
                    "AADHAAR fee not configured.");
            }

            var sessionUuid =
                Utils.GetUUID();

            var razorpayOrder =
                await _razorpayService
                    .CreateOrderAsync(
                        fee.Amount,
                        sessionUuid);

            await _aadhaarSessionRepository.DeactivateCustomerSessionsAsync(customerUuid);

            var session =
                new CustomerAadhaarSession
                {
                    UUID =
                        sessionUuid,

                    CustomerUUID =
                        customerUuid,

                    AadhaarNumberEncrypted =
                        _encryptionService.Encrypt(request.AadhaarNumber),

                    RazorpayOrderId =
                        razorpayOrder.OrderId,

                    Amount =
                        fee.Amount,

                    CreatedAt =
                        DateTimeOffset.UtcNow,

                    IsActive = true
                };

            await _aadhaarSessionRepository
                .AddAsync(
                    session);

            return new CreateAadhaarOrderResponseDto
            {
                SessionUUID =
                    sessionUuid,

                OrderId =
                    razorpayOrder.OrderId,

                Amount =
                    fee.Amount,

                RazorpayKeyId =
                    razorpayOrder.KeyId
            };
        }

        public async Task<AadhaarPaymentSuccessResponseDto> AadhaarPaymentSuccessAsync(
            string customerUuid,
            AadhaarPaymentSuccessRequestDto request)
        {
            var session =
                await _aadhaarSessionRepository
                    .GetByUUIDAsync(
                        request.SessionUUID);

            if (session == null)
            {
                throw new Exception(
                    "Invalid Aadhaar session.");
            }

            if (!session.IsActive)
            {
                throw new Exception(
                    "Aadhaar session expired.");
            }

            if (session.IsPaymentCompleted)
            {
                throw new Exception(
                    "Payment already processed.");
            }

            var isValidSignature =
                _razorpayService
                    .VerifySignature(
                        request.RazorpayOrderId,
                        request.RazorpayPaymentId,
                        request.RazorpaySignature);

            if (!isValidSignature)
            {
                throw new Exception(
                    "Invalid payment signature.");
            }

            session.IsPaymentCompleted = true;

            session.RazorpayPaymentId =
                request.RazorpayPaymentId;

            var decryptedAadhaarNumber = _encryptionService.Decrypt(session?.AadhaarNumberEncrypted);

            var aadhaarHash = Utils.GenerateHash(decryptedAadhaarNumber);

            var existingCustomer =
                await _customerService
                    .GetByAadhaarHashAsync(
                        aadhaarHash);

            if (existingCustomer != null
                &&
                existingCustomer.UUID != customerUuid
                &&
                existingCustomer.IsAadhaarVerified)
            {
                throw new Exception(
                    "Aadhaar already registered.");
            }

            var customer =
                await _customerService
                    .GetCustomerByUUID(
                        customerUuid);

            if (customer == null)
            {
                throw new Exception(
                    "Customer not found.");
            }

            customer.AadhaarHash = aadhaarHash;
            customer.CurrentStep = RegistrationSteps.VERIFY_AADHAAR;

            await _customerService.UpdateCustomerAsync(
                    customer);

            ValidateCurrentStep(
                customer.CurrentStep,
                RegistrationSteps.VERIFY_AADHAAR);

            var aadhaarResponse = await _aadhaarVerificationService.SendOtpAsync(decryptedAadhaarNumber);

            session.ClientId = aadhaarResponse.ClientId;

            session.RefId = Utils.GetUUID();

            session.IsOtpSent = true;

            session.OtpSentAt = DateTimeOffset.UtcNow;

            session.OtpResendCount = 0;

            session.VerifyAttemptCount = 0;

            session.LastResendAt =
                DateTimeOffset.UtcNow;

            await _aadhaarSessionRepository.UpdateAsync(session);

            return new AadhaarPaymentSuccessResponseDto
            {
                SessionUUID = session.UUID,

                ClientId = session.ClientId,

                RefId = session.RefId!,

                OtpSent = true,

                OtpPolicy = await BuildAadhaarOtpPolicyAsync(session)
            };
        }

        #endregion

        private async Task<RegistrationChallengeDto> CreateChallengeAsync(
            string customerUuid,
            string fullName)
        {
            var expirySeconds =
                await _appSettingService
                    .GetIntValueAsync(
                        AppSettingKeys
                            .VIDEO_KYC_CHALLENGE_EXPIRY_SECONDS,
                        60);

            var challengeCode =
                Random.Shared
                    .Next(
                        1000,
                        9999)
                    .ToString();

            var challengeTemplate =
                await _appSettingService
                    .GetValueAsync(
                        AppSettingKeys
                            .VIDEO_KYC_CHALLENGE_TEXT,
                        """
                My name is {FullName}. I confirm that the submitted information and documents belong to me. My authentication code is {ChallengeCode}.
                """);

            var challengeText =
                challengeTemplate
                    .Replace(
                        "{FullName}",
                        fullName,
                        StringComparison.OrdinalIgnoreCase)
                    .Replace(
                        "{ChallengeCode}",
                        challengeCode,
                        StringComparison.OrdinalIgnoreCase);

            var now =
                DateTimeOffset.UtcNow;

            // Deactivate previous unverified challenges
            await _customerVideoKycRepository
                .DeactivateActiveChallengesAsync(
                    customerUuid);

            var challenge =
                new CustomerVideoKyc
                {
                    UUID =
                        Utils.GetUUID(),

                    CustomerUUID =
                        customerUuid,

                    ChallengeText =
                        challengeText,

                    ChallengeCreatedAt =
                        now,

                    ChallengeExpiresAt =
                        now.AddSeconds(
                            expirySeconds),

                    IsVerified =
                        false,

                    IsActive =
                        true,

                    FaceDetected =
                        false,

                    SpokenText =
                        null,

                    FailureReason =
                        null
                };

            await _customerVideoKycRepository
                .AddAsync(
                    challenge);

            return new RegistrationChallengeDto
            {

                ChallengeText =
                    challenge.ChallengeText,

                ChallengeExpiresInSeconds = await _appSettingService.GetIntValueAsync(AppSettingKeys.VIDEO_KYC_RECORDING_DURATION_SECONDS, 30),

                ChallengeExpiresAtUtc =
                    challenge.ChallengeExpiresAt
            };
        }

        public async Task<GenerateVideoKycChallengeResponseDto>
    GenerateVideoKycChallengeAsync(
        string customerUuid)
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

            ValidateCurrentStep(
                customer.CurrentStep,
                RegistrationSteps.VIDEO_KYC);

            var challenge =
                await CreateChallengeAsync(customerUuid, $"{customer.FName} {customer.LName}");

            return new GenerateVideoKycChallengeResponseDto
            {
                Challenge = challenge
            };
        }

        public async Task<LocationLookupResponseDto> GetLocationAsync(
    decimal latitude,
    decimal longitude)
        {
            var url =
                $"https://nominatim.openstreetmap.org/reverse?format=jsonv2&lat={latitude}&lon={longitude}";

            _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(
                "AuthenticateIndia");

            var response =
                await _httpClient.GetStringAsync(
                    url);

            using var document =
                JsonDocument.Parse(
                    response);

            var address =
                document.RootElement
                    .GetProperty("address");

            var stateName =
                address.TryGetProperty(
                    "state",
                    out var stateElement)
                    ? stateElement.GetString()
                    : null;

            var cityName =
    address.TryGetProperty(
        "city",
        out var cityElement)
        ? cityElement.GetString()
        : address.TryGetProperty(
            "town",
            out var townElement)
            ? townElement.GetString()
            : address.TryGetProperty(
                "state_district",
                out var districtElement)
                ? districtElement.GetString()
                : address.TryGetProperty(
                    "village",
                    out var villageElement)
                    ? villageElement.GetString()
                    : null;

            if (string.IsNullOrWhiteSpace(stateName) ||
                string.IsNullOrWhiteSpace(cityName))
            {
                return new();
            }

            var states =
                await _stateService
                    .GetDropdownAsync(
                        x => x.Title);

            var state =
                states.FirstOrDefault(
                    x => x.Title.Equals(
                        stateName,
                        StringComparison.OrdinalIgnoreCase));

            if (state == null)
            {
                return new();
            }

            var cities =
                await _cityService
                    .GetCityDropDownWithFK();

            var city =
                cities.FirstOrDefault(
                    x =>
                        x.ForeignKey == state.UUID &&
                        x.Title != null &&
                        x.Title.Equals(
                            cityName,
                            StringComparison.OrdinalIgnoreCase));

            return new LocationLookupResponseDto
            {
                StateUUID =
                    state.UUID,

                CityUUID =
                    city?.UUID
            };
        }

        private async Task<OtpPolicyDto> BuildMobileOtpPolicyAsync(
            CustomerOtp? otp = null)
        {
            var maxResendCount =
                await _appSettingService
                    .GetIntValueAsync(
                        AppSettingKeys.OTP_MOBILE_MAX_RESEND_COUNT,
                        5);

            var maxVerifyAttempts =
                await _appSettingService
                    .GetIntValueAsync(
                        AppSettingKeys.OTP_MOBILE_MAX_VERIFY_ATTEMPTS,
                        5);

            var lockoutDurationSeconds =
    await _appSettingService
        .GetIntValueAsync(
            AppSettingKeys.OTP_MOBILE_LOCKOUT_SECONDS,
            900);

            var isLocked =
                otp != null
                &&
                (
                    otp.LockedUntil > DateTimeOffset.UtcNow
                    ||
                    otp.ResendCount >= maxResendCount
                );

            var lockedUntil =
                otp?.LockedUntil;

            if (
                otp != null
                &&
                otp.ResendCount >= maxResendCount
                &&
                lockedUntil == null)
            {
                lockedUntil =
                    otp.LastResendAt?
                        .AddSeconds(
                            lockoutDurationSeconds);
            }

            return new OtpPolicyDto
            {
                OtpExpiresInSeconds =
                    await _appSettingService
                        .GetIntValueAsync(
                            AppSettingKeys.OTP_MOBILE_EXPIRY_SECONDS,
                            60),

                MaxResendOtpCount =
                    maxResendCount,

                ResendCooldownSeconds =
                    await _appSettingService
                        .GetIntValueAsync(
                            AppSettingKeys.OTP_MOBILE_RESEND_COOLDOWN_SECONDS,
                            30),

                MaxVerifyAttemptCount =
                    maxVerifyAttempts,

                LockoutDurationSeconds =
                    await _appSettingService
                        .GetIntValueAsync(
                            AppSettingKeys.OTP_MOBILE_LOCKOUT_SECONDS,
                            900),

                RemainingResendOtpCount =
                    otp == null
                        ? maxResendCount
                        : Math.Max(
                            0,
                            maxResendCount - otp.ResendCount),

                RemainingVerifyAttemptCount =
                    otp == null
                        ? maxVerifyAttempts
                        : Math.Max(
                            0,
                            maxVerifyAttempts - otp.VerifyAttemptCount),

                IsLocked = isLocked,

                LockedUntil = lockedUntil
            };
        }

        private async Task<OtpPolicyDto> BuildAadhaarOtpPolicyAsync(
            CustomerAadhaarSession? session = null)
        {
            var maxResendCount =
                await _appSettingService
                    .GetIntValueAsync(
                        AppSettingKeys.OTP_AADHAAR_MAX_RESEND_COUNT,
                        3);

            var maxVerifyAttempts =
                await _appSettingService
                    .GetIntValueAsync(
                        AppSettingKeys.OTP_AADHAAR_MAX_VERIFY_ATTEMPTS,
                        5);

            return new OtpPolicyDto
            {
                OtpExpiresInSeconds =
                    await _appSettingService
                        .GetIntValueAsync(
                            AppSettingKeys.OTP_AADHAAR_EXPIRY_SECONDS,
                            300),

                MaxResendOtpCount =
                    maxResendCount,

                ResendCooldownSeconds =
                    await _appSettingService
                        .GetIntValueAsync(
                            AppSettingKeys.OTP_AADHAAR_RESEND_COOLDOWN_SECONDS,
                            30),

                MaxVerifyAttemptCount =
                    maxVerifyAttempts,

                LockoutDurationSeconds =
                    await _appSettingService
                        .GetIntValueAsync(
                            AppSettingKeys.OTP_AADHAAR_LOCKOUT_SECONDS,
                            900),

                RemainingResendOtpCount =
                    session == null
                        ? maxResendCount
                        : Math.Max(
                            0,
                            maxResendCount - session.OtpResendCount),

                RemainingVerifyAttemptCount =
                    session == null
                        ? maxVerifyAttempts
                        : Math.Max(
                            0,
                            maxVerifyAttempts - session.VerifyAttemptCount),

                IsLocked =
                    session?.LockedUntil >
                    DateTimeOffset.UtcNow,

                LockedUntil =
                    session?.LockedUntil
            };
        }


        private static void ValidateCurrentStep(
    string currentStep,
    string expectedStep)
        {
            if (!string.Equals(
                currentStep,
                expectedStep,
                StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException(
                    $"Expected step '{expectedStep}' but customer is at '{currentStep}'.");
            }
        }

        public async Task<ValidateReferralResponseDto> ValidateReferralAsync(
            string referralCode)
        {
            if (string.IsNullOrWhiteSpace(
                    referralCode))
            {
                return new()
                {
                    IsValid = false
                };
            }

            var agent =
                await _customerService
                    .GetReferralAgentAsync(
                        referralCode.Trim());

            if (agent == null)
            {
                return new()
                {
                    IsValid = false
                };
            }

            return new()
            {
                IsValid = true,
                AgentUUID = agent.UUID,
                AgentName =
                    $"{agent.FName} {agent.LName}"
            };
        }

        public async Task<SendOtpResponseDto> SendOtpAsync(
            SendOtpRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(
                    request.MobileNumber))
            {
                throw new ArgumentException(
                    "Mobile number is required.");
            }

            var customer =
                await _customerService
                    .GetByMobileAsync(
                        request.MobileNumber);

            if (customer == null)
            {
                customer =
                    await _customerService
                        .CreateCustomerAsync(
                            request.MobileNumber);
            }

            //
            // Apply referral only once
            //
            if (!string.IsNullOrWhiteSpace(
                    request.ReferralCode)
                &&
                string.IsNullOrWhiteSpace(
                    customer.ReferralCode))
            {
                var referralAgent =
                    await _customerService
                        .GetReferralAgentAsync(
                            request.ReferralCode.Trim());

                if (referralAgent == null)
                {
                    throw new Exception(
                        "Invalid referral code.");
                }

                if (referralAgent.Mobile ==
                    customer.Mobile)
                {
                    throw new Exception(
                        "You cannot refer yourself.");
                }

                customer.ReferralCode =
                    referralAgent.Mobile;

                await _customerService
                    .UpdateCustomerAsync(
                        customer);
            }

            var existingOtp =
                await _otpRepository
                    .GetLatestAsync(
                        customer.UUID);

            if (existingOtp != null)
            {
                //
                // User currently locked
                //
                if (
                    existingOtp.LockedUntil.HasValue
                    &&
                    existingOtp.LockedUntil >
                    DateTimeOffset.UtcNow)
                {
                    throw new Exception(
                        "Too many OTP attempts. Please try again later.");
                }

                //
                // Lock expired -> reset counters
                //
                if (
                    existingOtp.LockedUntil.HasValue
                    &&
                    existingOtp.LockedUntil <=
                    DateTimeOffset.UtcNow)
                {
                    existingOtp.ResendCount = 0;

                    existingOtp.VerifyAttemptCount = 0;

                    existingOtp.LockedUntil = null;

                    await _otpRepository
                        .UpdateAsync(
                            existingOtp);
                }

                var maxResendCount =
                    await _appSettingService
                        .GetIntValueAsync(
                            AppSettingKeys.OTP_MOBILE_MAX_RESEND_COUNT,
                            5);

                var lockoutSeconds =
                    await _appSettingService
                        .GetIntValueAsync(
                            AppSettingKeys.OTP_MOBILE_LOCKOUT_SECONDS,
                            900);

                //
                // Option A:
                // 1..5 allowed
                // 6th request locks user
                //
                if (existingOtp.ResendCount >= maxResendCount)
                {
                    existingOtp.LockedUntil =
                        DateTimeOffset.UtcNow
                            .AddSeconds(
                                lockoutSeconds);

                    await _otpRepository
                        .UpdateAsync(
                            existingOtp);

                    throw new Exception(
                        "Maximum OTP resend limit reached.");
                }

                var cooldownSeconds =
                    await _appSettingService
                        .GetIntValueAsync(
                            AppSettingKeys.OTP_MOBILE_RESEND_COOLDOWN_SECONDS,
                            30);

                if (
                    existingOtp.LastResendAt.HasValue
                    &&
                    existingOtp.LastResendAt.Value
                        .AddSeconds(
                            cooldownSeconds)
                        > DateTimeOffset.UtcNow)
                {
                    throw new Exception(
                        "Please wait before requesting another OTP.");
                }
            }

            await _otpService.SendOtpAsync(
                customer.UUID,
                customer.Mobile,
                request.Channel,
                existingOtp == null
                    ? 0
                    : existingOtp.ResendCount + 1,
                existingOtp?.VerifyAttemptCount ?? 0);

            var otp =
                await _otpRepository
                    .GetLatestAsync(
                        customer.UUID);

            return new SendOtpResponseDto
            {
                CustomerUUID =
                    customer.UUID,

                OtpPolicy =
                    await BuildMobileOtpPolicyAsync(
                        otp)
            };
        }

        public async Task<SendAadhaarOtpResponseDto> ResendAadhaarOtpAsync(
            string customerUuid,
            ResendAadhaarOtpRequestDto request)
        {
            var session =
                await _aadhaarSessionRepository
                    .GetActiveByCustomerUUIDAsync(
                        customerUuid);

            if (session == null)
            {
                throw new Exception(
                    "Invalid Aadhaar session.");
            }

            if (!session.IsActive)
            {
                throw new Exception(
                    "Aadhaar session expired.");
            }

            if (session.CustomerUUID != customerUuid)
            {
                throw new Exception(
                    "Invalid Aadhaar session.");
            }

            var isPaymentCompleted = await _aadhaarSessionRepository.HasCompletedPaymentAsync(customerUuid);

            if (!isPaymentCompleted)
            {
                throw new Exception(
                    "Payment pending.");
            }

            if (session.IsVerified)
            {
                throw new Exception(
                    "Aadhaar already verified.");
            }

            var aadhaarNumber =
                request.AadhaarNumber?.Trim();

            if (string.IsNullOrWhiteSpace(
                    aadhaarNumber))
            {
                throw new Exception(
                    "Aadhaar number is required.");
            }

            var existingAadhaar =
                string.IsNullOrWhiteSpace(
                    session.AadhaarNumberEncrypted)
                    ? string.Empty
                    : _encryptionService.Decrypt(
                        session.AadhaarNumberEncrypted);

            var isResend =
                string.Equals(
                    existingAadhaar,
                    aadhaarNumber,
                    StringComparison.Ordinal);

            if (isResend)
            {
                var maxResendCount =
                    await _appSettingService
                        .GetIntValueAsync(
                            AppSettingKeys.OTP_AADHAAR_MAX_RESEND_COUNT,
                            3);

                if (session.OtpResendCount >=
                    maxResendCount)
                {
                    throw new Exception(
                        "Maximum OTP resend limit reached.");
                }

                var cooldownSeconds =
                    await _appSettingService
                        .GetIntValueAsync(
                            AppSettingKeys.OTP_AADHAAR_RESEND_COOLDOWN_SECONDS,
                            30);

                if (
                    session.LastResendAt.HasValue
                    &&
                    session.LastResendAt.Value
                        .AddSeconds(
                            cooldownSeconds)
                        > DateTimeOffset.UtcNow)
                {
                    throw new Exception(
                        "Please wait before requesting another OTP.");
                }

                session.OtpResendCount++;

                session.LastResendAt =
                    DateTimeOffset.UtcNow;
            }
            else
            {
                var aadhaarHash =
                    Utils.GenerateHash(
                        aadhaarNumber);

                var existingCustomer =
                    await _customerService
                        .GetByAadhaarHashAsync(
                            aadhaarHash);

                if (existingCustomer != null
                    &&
                    existingCustomer.UUID != customerUuid
                    &&
                    existingCustomer.IsAadhaarVerified)
                {
                    throw new Exception(
                        "Aadhaar already registered.");
                }

                var customer =
                    await _customerService
                        .GetCustomerByUUID(
                            customerUuid);

                if (customer == null)
                {
                    throw new Exception(
                        "Customer not found.");
                }

                customer.AadhaarHash =
                    aadhaarHash;

                await _customerService
                    .UpdateCustomerAsync(
                        customer);

                session.AadhaarNumberEncrypted =
                    _encryptionService
                        .Encrypt(
                            aadhaarNumber);

                session.VerifyAttemptCount = 0;

                session.OtpResendCount = 0;

                session.LockedUntil = null;

                session.FailureReason = null;
            }

            var aadhaarResponse =
                await _aadhaarVerificationService
                    .SendOtpAsync(
                        aadhaarNumber);

            session.ClientId =
                aadhaarResponse.ClientId;

            session.RefId =
                Utils.GetUUID();

            session.IsOtpSent = true;

            session.OtpSentAt =
                DateTimeOffset.UtcNow;

            session.UpdatedAt =
                DateTimeOffset.UtcNow;

            await _aadhaarSessionRepository
                .UpdateAsync(
                    session);

            aadhaarResponse.OtpPolicy =
                await BuildAadhaarOtpPolicyAsync(
                    session);

            return aadhaarResponse;
        }

        public async Task<VerifyOtpResultDto> VerifyOtpAsync(
    VerifyOtpRequestDto request)
        {
            var customer =
                await _customerService
                    .GetByMobileAsync(
                        request.MobileNumber);

            if (customer == null)
            {
                return new VerifyOtpResultDto
                {
                    Success = false,
                    Message = "Customer not found."
                };
            }

            var otpResult =
                await _otpService
                    .VerifyOtpAsync(
                        customer.UUID,
                        request.Otp);

            var otp =
                await _otpRepository
                    .GetLatestAsync(
                        customer.UUID);

            if (!otpResult)
            {
                return new VerifyOtpResultDto
                {
                    Success = false,
                    Message = "Invalid OTP.",

                    Response =
                        new VerifyOtpResponseDto
                        {
                            CustomerUUID =
                                customer.UUID,

                            Mobile =
                                customer.Mobile,

                            CurrentStep =
                                customer.CurrentStep,

                            OtpPolicy =
                                await BuildMobileOtpPolicyAsync(
                                    otp)
                        }
                };
            }

            var token =
                await _jwtTokenService
                    .GenerateCustomerTokenAsync(
                        customer);

            customer.Mobile =
                request.MobileNumber;

            await _customerService
                .UpdateCustomerAsync(
                    customer);

            return new VerifyOtpResultDto
            {
                Success = true,

                Message = "OTP verified successfully.",

                Response =
                    new VerifyOtpResponseDto
                    {
                        CustomerUUID =
                            customer.UUID,

                        Mobile =
                            customer.Mobile,

                        CurrentStep =
                            customer.CurrentStep,

                        AccessToken =
                            token.AccessToken,

                        RefreshToken =
                            token.RefreshToken,

                        AccessTokenExpiresAtUtc =
                            token.AccessTokenExpiresAtUtc
                            ?? DateTime.UtcNow,

                        RefreshTokenExpiresAtUtc =
                            token.RefreshTokenExpiresAtUtc
                            ?? DateTime.UtcNow,

                        RegistrationState =
                            await _stateBuilder
                                .BuildAsync(
                                    customer.UUID),

                        OtpPolicy =
                            await BuildMobileOtpPolicyAsync(
                                otp)
                    }
            };
        }

        public async Task<RegistrationStateDto>
            GetRegistrationStateAsync(
                string customerUuid)
        {
            return await _stateBuilder
                .BuildAsync(customerUuid);
        }

        public async Task<VerifyAadhaarOtpWorkflowResponseDto> VerifyAadhaarOtpAsync(
            string customerUuid,
            VerifyAadhaarOtpRequestDto request)
        {
            CustomerAadhaarSession? session = null;

            try
            {
                session =
                    await _aadhaarSessionRepository
                        .GetByUUIDAsync(
                            request.SessionUUID);

                if (session == null)
                {
                    throw new Exception(
                        "Aadhaar session not found.");
                }

                if (!session.IsActive)
                {
                    throw new Exception(
                        "Aadhaar session expired.");
                }

                if (session.CustomerUUID != customerUuid)
                {
                    throw new Exception(
                        "Invalid Aadhaar session.");
                }

                if (session.IsVerified)
                {
                    throw new Exception(
                        "Aadhaar already verified.");
                }

                if (!session.IsPaymentCompleted)
                {
                    throw new Exception(
                        "Payment pending.");
                }

                if (!session.IsOtpSent)
                {
                    throw new Exception(
                        "OTP not generated.");
                }

                if (string.IsNullOrWhiteSpace(session.RefId) ||
                    string.IsNullOrWhiteSpace(session.ClientId))
                {
                    throw new Exception(
                        "Invalid Aadhaar session.");
                }

                if (session.LockedUntil > DateTimeOffset.UtcNow)
                {
                    throw new Exception(
                        "Too many OTP attempts. Please try again later.");
                }

                var aadhaarOtpExpirySeconds = await _appSettingService.GetIntValueAsync(AppSettingKeys.OTP_AADHAAR_EXPIRY_SECONDS, 300);

                if (
                    session.OtpSentAt.HasValue
                    &&
                    session.OtpSentAt.Value.AddSeconds(aadhaarOtpExpirySeconds) < DateTimeOffset.UtcNow)
                {
                    throw new Exception(
                        "OTP expired.");
                }

                var response =
                    await _aadhaarVerificationService
                        .VerifyOtpAsync(
                            session.RefId,
                            session.ClientId,
                            request.Otp);

                if (!response.Success)
                {
                    var maxAttempts =
                        await _appSettingService
                            .GetIntValueAsync(
                                AppSettingKeys.OTP_AADHAAR_MAX_VERIFY_ATTEMPTS,
                                5);

                    var lockoutSeconds =
                        await _appSettingService
                            .GetIntValueAsync(
                                AppSettingKeys.OTP_AADHAAR_LOCKOUT_SECONDS,
                                900);

                    session.VerifyAttemptCount++;

                    if (session.VerifyAttemptCount >= maxAttempts)
                    {
                        session.LockedUntil =
                            DateTimeOffset.UtcNow
                                .AddSeconds(
                                    lockoutSeconds);
                    }

                    session.UpdatedAt =
                        DateTimeOffset.UtcNow;

                    await _aadhaarSessionRepository
                        .UpdateAsync(
                            session);

                    throw new Exception(
                        "Invalid OTP.");
                }

                var originalAadhaar =
                    _encryptionService.Decrypt(session.AadhaarNumberEncrypted);

                if (!string.Equals(
                        originalAadhaar,
                        response.AadhaarNumber,
                        StringComparison.Ordinal))
                {
                    throw new Exception(
                        "Aadhaar mismatch.");
                }

                var customer =
                    await _customerService
                        .GetCustomerByUUID(
                            customerUuid);

                if (customer == null)
                {
                    throw new Exception(
                        "Customer not found.");
                }

                var parts =
                    response.FullName
                        .Trim()
                        .Split(
                            ' ',
                            StringSplitOptions.RemoveEmptyEntries);

                customer.FName =
                    parts.FirstOrDefault()
                    ?? string.Empty;

                customer.LName =
                    parts.Length > 1
                        ? string.Join(
                            " ",
                            parts.Skip(1))
                        : string.Empty;

                customer.IsAadhaarVerified = true;

                customer.CurrentStep =
                    RegistrationSteps.BASIC_INFO;

                session.VerifyAttemptCount = 0;

                session.LockedUntil = null;

                session.IsVerified = true;

                session.IsActive = false;

                session.VerifiedAt =
                    DateTimeOffset.UtcNow;

                session.UpdatedAt =
                    DateTimeOffset.UtcNow;

                var latLong = "";

                var customerDocument =
                    new CustomerRegDocument
                    {
                        UUID =
                            Utils.GetUUID(),

                        CustomerUUID =
                            customerUuid,

                        RecordCategory =
                            CustomerRegDocumentType.AADHAAR,

                        RecordType =
                            customer.ACType,

                        RecordNo =
                            _encryptionService
                                .Encrypt(
                                    response.AadhaarNumber),

                        IsVerified =
                            true,

                        VerificationTimeStamp =
                            DateTimeOffset.UtcNow,

                        VerifiedFrom =
                            "WEB",

                        // TODO:
                        // Store actual verification location
                        City = "",

                        //FIX: split lat long and then save with ,
                        Latitude = "",

                        Longitude = "",

                        IpAddress = _auditContext.IpAddress,

                        IsActive = true
                    };

                // Caution : Don't change the order of these operations as they are not wrapped in a transaction. We need to ensure session is updated before adding document and updating customer.
                await _aadhaarSessionRepository
                    .UpdateAsync(
                        session, false);

                await _customerService
                    .UpdateCustomerAsync(
                        customer, false);

                await _customerRegDocumentRepository
                    .AddAsync(
                        customerDocument, true);


                var state = await _stateBuilder.BuildAsync(customerUuid);

                return new VerifyAadhaarOtpWorkflowResponseDto
                {
                    Success = true,
                    Message = "Aadhaar verification successful.",
                    RegistrationState = state,
                    OtpPolicy = await BuildAadhaarOtpPolicyAsync(session)
                };
            }
            catch (Exception ex)
            {
                if (session != null)
                {
                    session.FailureReason =
                        ex.Message;

                    session.UpdatedAt =
                        DateTimeOffset.UtcNow;

                    await _aadhaarSessionRepository
                        .UpdateAsync(
                            session);
                }

                throw;
            }
        }

        public async Task<RegistrationStateDto> SaveBasicInfoAsync(
    string customerUuid,
    SaveBasicInfoRequestDto request)
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

            ValidateCurrentStep(
        customer.CurrentStep,
        RegistrationSteps.BASIC_INFO);

            customer.Email =
                request.Email;

            customer.StateUUID =
                request.StateUUID;

            customer.CityUUID =
                request.CityUUID;

            customer.IndustryUUID =
                request.IndustryUUID;

            customer.CurrentStep =
                RegistrationSteps.VIDEO_KYC;

            await _customerService
                .UpdateCustomerAsync(
                    customer);

            return await _stateBuilder.BuildAsync(customerUuid, generateKycChallenge: false);
        }

        public async Task<RegistrationStateDto> UploadVideoKycAsync(
            string customerUuid,
            UploadVideoKycRequestDto request)
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

            ValidateCurrentStep(
                customer.CurrentStep,
                RegistrationSteps.VIDEO_KYC);

            if (string.IsNullOrWhiteSpace(
                    request.VideoUrl))
            {
                throw new Exception(
                    "Video URL is required.");
            }

            var challenge =
                await _customerVideoKycRepository
                    .GetLatestChallengeAsync(
                        customerUuid);

            if (challenge == null)
            {
                throw new Exception(
                    "Video challenge not found.");
            }

            if (!challenge.ChallengeExpiresAt.HasValue)
            {
                throw new Exception(
                    "Invalid video challenge.");
            }

            if (challenge.ChallengeExpiresAt.Value <=
                DateTimeOffset.UtcNow)
            {
                throw new Exception(
                    "Video challenge expired.");
            }

            var verification =
                await _videoKycVerificationService
                    .VerifyAsync(
                        challenge,
                        request.VideoUrl);

            var isVerified =
                verification.IsMatched;

            challenge.VideoUrl =
                request.VideoUrl;

            challenge.SpokenText =
                verification.SpokenText;

            challenge.SpeechMatchPercentage =
                verification.SpeechMatchPercentage;

            challenge.FaceDetectionPercentage =
                verification.FaceDetectionPercentage;

            challenge.FaceDetected =
                verification.FaceDetected;

            challenge.IsVerified =
                isVerified;

            challenge.FailureReason =
                verification.FailureReason;

            challenge.VerificationTimeStamp =
                DateTimeOffset.UtcNow;

            challenge.IsActive = false;

            await _customerVideoKycRepository
                .UpdateAsync(
                    challenge);

            if (!isVerified)
            {
                throw new Exception(
                    challenge.FailureReason
                    ?? "Video verification failed.");
            }

            customer.CurrentStep =
                RegistrationSteps.TERMS;

            await _customerService
                .UpdateCustomerAsync(
                    customer);

            return await _stateBuilder
                .BuildAsync(
                    customerUuid);
        }

        public async Task<AcceptTermsResponseDto> AcceptTermsAsync(
    string customerUuid,
    AcceptTermsRequestDto request)
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

            ValidateCurrentStep(
                customer.CurrentStep,
                RegistrationSteps.TERMS);

            var existingConsent =
                await _customerConsentRepository
                    .GetLatestByCustomerUUIDAsync(
                        customerUuid);

            if (existingConsent != null)
            {
                return new AcceptTermsResponseDto
                {
                    SignId =
                        existingConsent.SignId
                        ?? string.Empty,

                    SignedAt =
                        existingConsent.SignedAt
                        ?? DateTimeOffset.UtcNow,

                    Name = existingConsent.Signature
                        ?? $"{customer.FName} {customer.LName}"
                };
            }

            if (!request.PolicyUUIDs.Any())
            {
                throw new Exception(
                    "No policies selected.");
            }

            var policies =
                await _policyService
                    .GetPolicyListForRegistrationAsync();

            var signId =
                Utils.GetUUID();

            var signedAt =
                DateTimeOffset.UtcNow;

            foreach (var policyUuid in request.PolicyUUIDs)
            {
                var policy =
                    policies.FirstOrDefault(
                        x => x.UUID == policyUuid);

                if (policy == null)
                {
                    continue;
                }

                var consent =
                    new CustomerConsent
                    {
                        UUID =
                            Utils.GetUUID(),

                        CustomerUUID =
                            customerUuid,

                        PolicyUUID =
                            policy.UUID,

                        PolicyVersion =
                            policy.Version,

                        PolicyContentHash =
                            policy.ContentHash,

                        ConsentGiven =
                            true,

                        SignedAt =
                            signedAt,

                        SignId =
                            signId,

                        Signature =
                            $"{customer.FName} {customer.LName}",

                        Platform =
                            "WEB",

                        LatLong = "",

                        IpAddress = Utils.GetLocalIPAddress()
                    };

                await _customerConsentRepository
                    .AddAsync(
                        consent);
            }

            return new AcceptTermsResponseDto
            {
                SignId =
                    signId,

                SignedAt =
                    signedAt,

                Name = $"{customer.FName} {customer.LName}"
            };
        }

        public async Task<RegistrationStateDto> CompleteTermsAsync(
    string customerUuid)
        {
            var consent =
                await _customerConsentRepository
                    .GetLatestByCustomerUUIDAsync(
                        customerUuid);

            if (consent == null)
            {
                throw new Exception(
                    "Please sign policies first.");
            }

            var customer =
                await _customerService
                    .GetCustomerByUUID(
                        customerUuid);

            if (customer == null)
            {
                throw new Exception(
                    "Customer not found.");
            }

            customer.CurrentStep =
                RegistrationSteps.DOS_DONTS;

            await _customerService
                .UpdateCustomerAsync(
                    customer);

            return await _stateBuilder.BuildAsync(customerUuid);
        }

        public async Task<RegistrationStateDto> SaveReferralAsync(
    string customerUuid,
    SaveReferralRequestDto request)
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

            ValidateCurrentStep(
                customer.CurrentStep,
                RegistrationSteps.DOS_DONTS);

            //
            // Skip
            //
            if (string.IsNullOrWhiteSpace(
                request.ReferralCode))
            {
                customer.CurrentStep =
                    RegistrationSteps.DOS_DONTS;

                await _customerService
    .UpdateCustomerAsync(
        customer);

                return await _stateBuilder
                    .BuildAsync(
                        customerUuid);
            }

            var referralAgent =
                await _customerService
                    .GetReferralAgentAsync(
                        request.ReferralCode.Trim());

            if (referralAgent == null)
            {
                throw new Exception(
                    "Invalid referral code.");
            }

            if (referralAgent.UUID ==
                customer.UUID)
            {
                throw new Exception(
                    "You cannot refer yourself.");
            }

            customer.ReferralCode =
                referralAgent.Mobile;

            customer.CurrentStep =
                RegistrationSteps.DOS_DONTS;

            await _customerService
    .UpdateCustomerAsync(
        customer);

            return await _stateBuilder
                .BuildAsync(
                    customerUuid);
        }


        public async Task<RegistrationStateDto> CompleteRegistrationAsync(string customerUuid)
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

            ValidateCurrentStep(
    customer.CurrentStep,
    RegistrationSteps.DOS_DONTS);

            customer.CurrentStep =
                RegistrationSteps.DASHBOARD;

            customer.IsRegistered = true;

            customer.RegTimeStamp =
                DateTimeOffset.UtcNow;

            await _customerService
                .UpdateCustomerAsync(
                    customer);

            return await _stateBuilder
    .BuildAsync(
        customerUuid);
        }

        public async Task<RegistrationStateDto> AcceptDosDontsAsync(
            string customerUuid,
            AcceptDosDontsRequestDto request)
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

            ValidateCurrentStep(
                customer.CurrentStep,
                RegistrationSteps.DOS_DONTS);

            if (!request.ConsentGiven)
            {
                throw new Exception(
                    "Consent is required.");
            }

            var activeDocument =
                await _masterDosDontsDocumentRepository
                    .GetActiveAsync();

            if (activeDocument == null)
            {
                throw new Exception(
                    "No active Do's & Don'ts document found.");
            }

            if (!string.Equals(
                    request.DocumentUUID,
                    activeDocument.UUID,
                    StringComparison.OrdinalIgnoreCase))
            {
                throw new Exception(
                    "The Do's & Don'ts have changed. Please reload and review the latest version.");
            }

            var consent =
                await _customerConsentDosDontsRepository
                    .GetByCustomerUUIDAsync(
                        customerUuid);

            if (consent == null)
            {
                consent = new CustomerConsentDosDonts
                {
                    UUID = Utils.GetUUID(),
                    CustomerUUID = customerUuid,
                    DocumentUUID = activeDocument.UUID,
                    ConsentGiven = true,
                    ConsentGivenAt = DateTimeOffset.UtcNow,
                    IsActive = true,
                };

                await _customerConsentDosDontsRepository.AddAsync(consent);
            }
            else
            {
                consent.DocumentUUID = activeDocument.UUID;
                consent.ConsentGiven = true;
                consent.ConsentGivenAt = DateTimeOffset.UtcNow;
                consent.IsActive = true;

                await _customerConsentDosDontsRepository.UpdateAsync(consent);
            }

            /*var httpContext =
                _httpContextAccessor.HttpContext;*/

            consent.DocumentUUID =
                activeDocument.UUID;

            consent.ConsentGiven =
                true;

            consent.ConsentGivenAt =
                DateTimeOffset.UtcNow;

            consent.IPAddress = "";

            consent.Platform = "";

            consent.IsActive =
                true;

            await _customerConsentDosDontsRepository
                .UpdateAsync(
                    consent);

            customer.IsRegistered =
                true;

            customer.CurrentStep =
                RegistrationSteps.DASHBOARD;

            await _customerService
                .UpdateCustomerAsync(
                    customer);

            return await _stateBuilder
                .BuildAsync(
                    customerUuid);
        }
    }
}
