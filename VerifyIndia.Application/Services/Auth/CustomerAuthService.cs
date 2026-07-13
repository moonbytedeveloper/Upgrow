using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Cryptography;
using System.Text.Json;
using VerifyIndia.Application.DTO.Auth;
using VerifyIndia.Application.IServices.Auth;
using VerifyIndia.Domain.Entities;
using VerifyIndia.Domain.IRepositories;
using VerifyIndia.Domain.IRepositories.Master;

namespace VerifyIndia.Application.Services.Auth
{
    public class CustomerAuthService : ICustomerAuthService
    {
        private const int AllowedSkewSeconds = 300; // 5 minutes        
        private const int VideoKycChallengeExpiryMinutes = 5;

        private readonly ITenantRepository _tenantRepository;
        private readonly IMasterCustomerRepository _masterCustomerRepository;
        private readonly ICustomerVideoKycRepository _customerVideoKycRepository;
        private readonly IHmacService _hmacService;
        private readonly IMemoryCache _memoryCache;
        private readonly IRegistrationApiLogService _registrationApiLogService;

        public CustomerAuthService(
            ITenantRepository clientRepository,
            IMasterCustomerRepository masterCustomerRepository,
            ICustomerVideoKycRepository customerVideoKycRepository,
            IHmacService hmacService,
            IMemoryCache memoryCache,
            IRegistrationApiLogService registrationApiLogService)
        {
            _tenantRepository = clientRepository;
            _masterCustomerRepository = masterCustomerRepository;
            _customerVideoKycRepository = customerVideoKycRepository;
            _hmacService = hmacService;
            _memoryCache = memoryCache;
            _registrationApiLogService = registrationApiLogService;
        }
        #region Helper Methods

        private async Task<HmacValidationResultDto> ValidateHmacRequestInternalAsync(ApiLoginHeaderDto headerDto, ApiLoginRequestDto requestDto)
        {
            if (string.IsNullOrWhiteSpace(headerDto.TenantIdentifier) ||
                string.IsNullOrWhiteSpace(headerDto.Timestamp) ||
                string.IsNullOrWhiteSpace(headerDto.Signature) ||
                string.IsNullOrWhiteSpace(headerDto.LatLong) ||
                string.IsNullOrWhiteSpace(headerDto.IpAddress))
            {
                return new HmacValidationResultDto
                {
                    IsValid = false,
                    ErrorMessage = "Invalid HMAC headers."
                };
            }

            if (!long.TryParse(headerDto.Timestamp, out var requestTimestamp))
            {
                return new HmacValidationResultDto
                {
                    IsValid = false,
                    ErrorMessage = "Invalid timestamp format."
                };
            }

            var now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            if (Math.Abs(now - requestTimestamp) > AllowedSkewSeconds)
            {
                return new HmacValidationResultDto
                {
                    IsValid = false,
                    ErrorMessage = "Timestamp expired."
                };
            }

            var tenant = await _tenantRepository.GetTenantDetailsByIdentifierAsync(headerDto.TenantIdentifier);
            if (tenant == null || string.IsNullOrWhiteSpace(tenant.SecretHash))
            {
                return new HmacValidationResultDto
                {
                    IsValid = false,
                    ErrorMessage = "Invalid client."
                };
            }

            var rawBody = JsonSerializer.Serialize(requestDto, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            var isValidSignature = _hmacService.ValidateSignature(headerDto.Signature, requestTimestamp, rawBody, tenant.SecretHash);
            if (!isValidSignature)
            {
                return new HmacValidationResultDto
                {
                    IsValid = false,
                    ErrorMessage = "Invalid signature."
                };
            }

            return new HmacValidationResultDto
            {
                IsValid = true,
                TenantIdentifier = tenant.Identifier,
                RawBody = rawBody
            };
        }
      

        private static readonly List<string> Sentences = new()
        {
            "My name is {name} and today is {date}.",
            "I confirm this is my video verification on {date}.",
            "This verification is being done by {name}.",
            "I am verifying my identity on {date}."
        };

        #endregion

        public Task<HmacValidationResultDto> ValidateHmacRequestAsync(ApiLoginHeaderDto headerDto, ApiLoginRequestDto requestDto)
            => ValidateHmacRequestInternalAsync(headerDto, requestDto);

        public Task<string> GenerateAndStoreOtpAsync(string mobileNo, string tenantIdentifier, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(mobileNo))
                throw new ArgumentException("Mobile number is required.", nameof(mobileNo));

            if (string.IsNullOrWhiteSpace(tenantIdentifier))
                throw new ArgumentException("Tenant id is required.", nameof(tenantIdentifier));

            var normalizedMobile = mobileNo.Trim();
            var normalizedTenant = tenantIdentifier.Trim();

            var otp = RandomNumberGenerator.GetInt32(1000, 10000).ToString();           

            return Task.FromResult(otp);
        }

        public async Task<bool> IsOtpRateLimitedAsync(string mobileNo, int tenantId, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(mobileNo) || tenantId <= 0)
            {
                return false;
            }

            var normalizedMobile = mobileNo.Trim();
            var sinceUtc = DateTimeOffset.UtcNow.AddHours(-1);

            var count = await _registrationApiLogService.CountAsync("/api/v1/send-mobile-otp", normalizedMobile, tenantId, sinceUtc);

            return count >= 5;
        }       

        public async Task<Master_Customer> RegisterCustomer(string mobileNo, string tenantIdentifier, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(mobileNo))
                throw new ArgumentException("Mobile number is required.", nameof(mobileNo));

            if (string.IsNullOrWhiteSpace(tenantIdentifier))
                throw new ArgumentException("Tenant id is required.", nameof(tenantIdentifier));
            Master_Customer? customer = null;

            var tenant = await _tenantRepository.GetTenantDetailsByIdentifierAsync(tenantIdentifier);

            customer = new Master_Customer
            {
                UUID = Utils.GetUUID(),
                Mobile = mobileNo,
                TenantId = tenant.Id,
                IsRegistered = false,
                CurrentStep = Constants.NextActions.SELECT_ACCOUNT_TYPE,
                IsActive = true
            };

            await _masterCustomerRepository.AddAsync(customer);

            return customer;
        }

        private string GenerateKycChallenge(string name, CancellationToken cancellationToken = default)
        {
            var random = RandomNumberGenerator.GetInt32(0, Sentences.Count);
            var template = Sentences[random];
            var normalizedName = string.IsNullOrWhiteSpace(name) ? "Customer" : name.Trim();

            return template
                .Replace("{name}", normalizedName)
                .Replace("{date}", DateTime.UtcNow.ToString("dd MMM yyyy"));
        }

        public async Task<(string, DateTime)> GenerateAndStoreKycChallengeAsync(string customerUuid, string customerName, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(customerUuid))
                throw new ArgumentException("Customer UUID is required.", nameof(customerUuid));

            var challenge = GenerateKycChallenge(customerName, cancellationToken);
            
            var record = new CustomerVideoKYC
            {
                UUID = Utils.GetUUID(),
                CustomerUUID = customerUuid,
                KycText = challenge,
                TimeStamp = DateTimeOffset.UtcNow,
                IsKycDone = false,
                IsTimeout = false,
                VideoFileUrl = null,
                OldRecordUUID = null
            };
            var now = DateTime.UtcNow;
            var expiresAt = now.AddMinutes(VideoKycChallengeExpiryMinutes);
            await _customerVideoKycRepository.AddAsync(record);
            return (challenge,expiresAt);
        }

        public async Task<(bool IsSuccess, string Message)> VerifyVideoKycAsync(string customerUuid, string kycText, string? videoFileUrl, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(customerUuid))
                return (false, "Customer UUID is required.");

            if (string.IsNullOrWhiteSpace(kycText))
                return (false, "KYC text is required.");

            var latestRecord = await _customerVideoKycRepository.GetLatestByCustomerUuidAsync(customerUuid.Trim());
            if (latestRecord == null)
                return (false, "KYC challenge not found.");

            if (latestRecord.IsKycDone)
                return (false, "KYC is already completed.");

            var isExpired = DateTimeOffset.UtcNow > latestRecord.TimeStamp.AddMinutes(VideoKycChallengeExpiryMinutes);
            if (isExpired)
            {
                latestRecord.IsTimeout = true;
                await _customerVideoKycRepository.UpdateAsync(latestRecord);
                return (false, "KYC challenge expired. Please generate a new challenge.");
            }

            var expected = latestRecord.KycText?.Trim();
            var provided = kycText.Trim();

            if (!string.Equals(expected, provided, StringComparison.OrdinalIgnoreCase))
                return (false, "Provided KYC text does not match generated challenge.");

            latestRecord.VideoFileUrl = string.IsNullOrWhiteSpace(videoFileUrl) ? latestRecord.VideoFileUrl : videoFileUrl.Trim();
            latestRecord.IsKycDone = true;
            latestRecord.IsTimeout = false;
            await _customerVideoKycRepository.UpdateAsync(latestRecord);

            return (true, "Video KYC verified successfully.");
        }
    }
}

