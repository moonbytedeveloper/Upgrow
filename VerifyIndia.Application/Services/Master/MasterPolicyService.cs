using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using VerifyIndia.Application.DTO.Auth;
using VerifyIndia.Application.DTO.Customer;
using VerifyIndia.Application.DTO.Master;
using VerifyIndia.Application.IServices.Auth;
using VerifyIndia.Application.IServices.Master;
using VerifyIndia.Application.Utilities;
using VerifyIndia.Domain.Entities;
using VerifyIndia.Domain.IRepositories.Master;

namespace VerifyIndia.Application.Services.Master
{
    public class MasterPolicyService : IMasterPolicyService
    {
        private readonly IMasterPolicyRepository _masterPolicyRepository;
        private readonly ICustomerConsentRepository _customerConsentRepository;
        private readonly IHmacService _hmacService;

        private static string GenerateSignature(long unixTimestampSeconds, string body, string key)
        {
            var payload = $"{unixTimestampSeconds}:{body}";
            var keyBytes = Encoding.UTF8.GetBytes(key);
            var payloadBytes = Encoding.UTF8.GetBytes(payload);

            using var hmac = new HMACSHA256(keyBytes);
            var hash = hmac.ComputeHash(payloadBytes);
            return Convert.ToHexString(hash).ToLowerInvariant();
        }
        public MasterPolicyService(IMasterPolicyRepository masterPolicyRepository, ICustomerConsentRepository customerConsentRepository, IHmacService hmacService)
        {
            _masterPolicyRepository = masterPolicyRepository;
            _customerConsentRepository = customerConsentRepository;
            _hmacService = hmacService;
        }
      
        public async Task<List<PolicyListDto>> GetPolicyListForRegistrationAsync()
        {            
            var codes = new List<string>
            {
                "RegistrationLegalAgreement",
                "RegistrationKyc",
                "RegistrationFraudDeclaration"
            };
            var  entities = await _masterPolicyRepository.GetPolicyListAsync(codes);          

            return entities                
                .Select(x => new PolicyListDto
                {
                    UUID = x.UUID!,
                    Title = x.Title!,
                    PolicyContent = x.PolicyContent!,
                    Version = x.Version!,
                    Code = x.Code!,
                    SequenceNo = x.SequenceNo,
                    ContentHash = x.ContentHash!,
                    IsActive = x.IsActive,
                    FromDate = x.FromDate,
                    ToDate = x.ToDate
                })
                .ToList();
        }
        public async Task<string> VerifyPolicyContent(PolicyConsentItemDto content)
        {
            var entity = await _masterPolicyRepository.GetByUuidAsync(content.PolicyUuid);
            if (entity == null)
                throw new ArgumentException("Policy not found");
            var contentHash = _hmacService.GenerateHash(content.PolicyContent);
            if(contentHash != entity.ContentHash)
                throw new ArgumentException("The policy content has changed or is invalid.");

            return contentHash;
        }

        public async Task<MasterPolicyDto?> GetByUuidAsync(string uuid)
        {
            if (string.IsNullOrWhiteSpace(uuid))
                return null;

            var entity = await _masterPolicyRepository.GetByUuidAsync(uuid.Trim());
            if (entity == null)
                return null;

            return new MasterPolicyDto
            {
                UUID = entity.UUID,
                Title = entity.Title,
                PolicyContent = entity.PolicyContent,
                Version = entity.Version,
                Code = entity.Code,
                SequenceNo = entity.SequenceNo,
                IsActive = entity.IsActive
            };
        }
        public async Task SaveConsentAsync(
            CustomerConsentDto consentDto)
        {
            if (string.IsNullOrWhiteSpace(consentDto.CustomerUUID))
                throw new ArgumentException("Customer UUID is required.", nameof(consentDto.CustomerUUID));

            if (string.IsNullOrWhiteSpace(consentDto.PolicyUUID))
                throw new ArgumentException("Policy UUID is required.", nameof(consentDto.PolicyUUID));

            if (string.IsNullOrWhiteSpace(consentDto.PolicyVersion))
                throw new ArgumentException("Policy version is required.", nameof(consentDto.PolicyVersion));           

            if (string.IsNullOrWhiteSpace(consentDto.PolicyContentHash))
                throw new ArgumentException("Policy content hash is required.", nameof(consentDto.PolicyContentHash));

            if (string.IsNullOrWhiteSpace(consentDto.IpAddress))
                throw new ArgumentException("IP address is required.", nameof(consentDto.IpAddress));

            if (string.IsNullOrWhiteSpace(consentDto.Platform))
                throw new ArgumentException("Platform is required.", nameof(consentDto.Platform));

            var privateKeyFilePath = await KeyPathResolver.GetPrivateKeyAsync();

           
            string consentDtoString = JsonSerializer.Serialize(consentDto);
            long timestamp = consentDto.SignedAt?.ToUnixTimeSeconds() ?? 0;
            var Signature = GenerateSignature(timestamp, consentDtoString, "YourSecretKey");
            var consent = new CustomerConsent
            {
                UUID = Utils.GetUUID(),
                CustomerUUID = consentDto.CustomerUUID,
                PolicyUUID = consentDto.PolicyUUID,                
                PolicyContentHash = consentDto.PolicyContentHash,
                PolicyVersion = consentDto.PolicyVersion,
                ConsentGiven = consentDto.ConsentGiven,
                SignedAt = consentDto.SignedAt,
                IpAddress = consentDto.IpAddress,
                Platform = consentDto.Platform,
                SignId = consentDto.SignId,
                Signature = Signature,
                LatLong = consentDto.LatLong
            };

            await _customerConsentRepository.AddAsync(consent);
            return;
        }

        public async Task<List<PolicyListDto>> GetActivePrivacyAndTermsAsync()
        {
            var entities = await _masterPolicyRepository.GetActivePrivacyAndTermsAsync();

            return entities
                .OrderBy(x => x.SequenceNo)
                .Select(x => new PolicyListDto
                {
                    UUID = x.UUID,
                    Title = x.Title,
                    PolicyContent = x.PolicyContent,
                    Version = x.Version,
                    Code = x.Code,
                    SequenceNo = x.SequenceNo
                })
                .ToList();
        }

        public async Task<PolicyListDto?> GetByCodeAsync(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                return null;

            var entity = await _masterPolicyRepository.GetByCodeLatestAsync(code);
            if (entity == null)
                return null;

            return new PolicyListDto
            {
                UUID = entity.UUID,
                Title = entity.Title,
                PolicyContent = entity.PolicyContent,
                Version = entity.Version,
                Code = entity.Code,
                SequenceNo = entity.SequenceNo
            };
        }



        public async Task UpdatePolicyVersionAsync(
    string selectedPolicyUuid,
    string bumpType,
    DateTime effectiveFromDate,
    string? newPolicyContent = null,
    int? newSequenceNo = null)
        {
            if (string.IsNullOrWhiteSpace(selectedPolicyUuid))
                throw new ArgumentException("Selected policy UUID is required.", nameof(selectedPolicyUuid));

            if (string.IsNullOrWhiteSpace(bumpType))
                throw new ArgumentException("Version type is required (\"minor\" or \"major\").", nameof(bumpType));

            var selected = await _masterPolicyRepository.GetByUuidAsync(selectedPolicyUuid.Trim());

            if (selected == null)
                throw new KeyNotFoundException("Selected policy not found.");

            if (!selected.IsActive)
                throw new InvalidOperationException("Only active policy records can be version-bumped.");

            // Validate effective date
            if (effectiveFromDate <= selected.FromDate)
            {
                throw new InvalidOperationException(
                    $"Effective From Date must be greater than {selected.FromDate:dd/MM/yyyy}");
            }

            // compute new version
            var currentVersion = selected.Version?.Trim() ?? "1";
            string newVersion;

            var parts = currentVersion.Split('.', StringSplitOptions.RemoveEmptyEntries);

            bool isNumericMajor = int.TryParse(parts[0], out var major);

            if (!isNumericMajor)
            {
                major = 1;
            }

            if (bumpType.Equals("minor", StringComparison.OrdinalIgnoreCase))
            {
                int minor = 0;

                if (parts.Length > 1 && int.TryParse(parts[1], out var parsedMinor))
                {
                    minor = parsedMinor + 1;
                }
                else
                {
                    minor = 1;
                }

                newVersion = $"{major}.{minor}";
            }
            else if (bumpType.Equals("major", StringComparison.OrdinalIgnoreCase))
            {
                var newMajor = major + 1;
                newVersion = newMajor.ToString();
            }
            else
            {
                throw new ArgumentException(
                    "Version type must be either \"minor\" or \"major\".",
                    nameof(bumpType));
            }

            // ensure no existing record already uses the new version for same code
            var existing = await _masterPolicyRepository
                .GetByCodeAndVersionAsync(selected.Code, newVersion);

            if (existing != null)
            {
                throw new InvalidOperationException(
                    $"A policy with code '{selected.Code}' and version '{newVersion}' already exists.");
            }

            // Create new version record
            var newEntity = new Master_Policy
            {
                UUID = Utils.GetUUID(),
                Title = selected.Title,
                PolicyContent = string.IsNullOrWhiteSpace(newPolicyContent)
                    ? selected.PolicyContent
                    : newPolicyContent.Trim(),
                Version = newVersion,
                Code = selected.Code,
                SequenceNo = newSequenceNo ?? selected.SequenceNo,

                // New fields
                FromDate = effectiveFromDate,
                ToDate = null,

                IsActive = true
            };

            // Insert new version
            await _masterPolicyRepository.AddAsync(newEntity);

            // Close previous version automatically
            selected.ToDate = effectiveFromDate.AddDays(-1);

            // Keep your existing functionality
            selected.IsActive = false;

            await _masterPolicyRepository.UpdateAsync(selected);
        }
    }
}
