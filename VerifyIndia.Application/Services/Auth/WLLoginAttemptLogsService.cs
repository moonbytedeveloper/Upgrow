using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using VerifyIndia.Application.DTO.Auth;
using VerifyIndia.Application.IServices.Auth;
using VerifyIndia.Application.Utilities;
using VerifyIndia.Domain.Common;
using VerifyIndia.Domain.Entities;
using VerifyIndia.Domain.IRepositories;

namespace VerifyIndia.Application.Services.Auth
{
    public class WLLoginAttemptLogsService : IWLLoginAttemptLogsService
    {
        private readonly IWLLoginAttemptRepository _repository;
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _env;

        public WLLoginAttemptLogsService(IWLLoginAttemptRepository repository, IConfiguration config, IWebHostEnvironment env)
        {
            _repository = repository;
            _config = config;
            _env = env;
        }

        public async Task AddAsync(LoginAttemptDto dto)
        {
            var json = JsonSerializer.Serialize(dto);

            var previousHash = await _repository.GetLastCurrentHashAsync();
            var currentHash = ActionLogHashUtility.ComputeCurrentHash(json);
            var privateKeyPath = KeyPathResolver.GetPrivateKeyPath();

            byte[]? signature = null;
            try
            {
                signature = ActionLogHashUtility.ComputeDigitalSignature(currentHash, privateKeyPath);
            }
            catch
            {
                // keep behavior consistent with action logging (signature optional if key fails)
            }

            var attempt = new WL_LoginAttempts
            {
                Payload = json,
                PreviousHash = previousHash,
                CurrentHash = currentHash,
                DigitalSignature = signature
            };

            await _repository.AddAsync(attempt);
        }

        public async Task<PagedResult<LoginAttemptDto>> GetPagedAsync(
           PaginationParams pagination,
           string userUuid,
           string? search = null)
        {
            try
            {
                var pageSize = pagination.PageSize > 0 ? pagination.PageSize : 10;

                // Get paginated raw data from repository (database-level filtering/pagination)
                var rawResult = await _repository.GetPagedRawAsync(pagination);

                // Deserialize all payloads
                var dtos = rawResult.Items
                    .Select(x => DeserializeLoginAttemptDto(x.Payload))
                    .Where(x => x != null)
                    .Cast<LoginAttemptDto>()
                    .OrderByDescending(x => x.AttemptTime)  // Most recent first
                    .ToList();

                // Apply userUuid filter
                if (!string.IsNullOrWhiteSpace(userUuid))
                {
                    dtos = dtos
                        .Where(x => x.UserUUID == userUuid)
                        .ToList();
                }

                // Apply search filter
                if (!string.IsNullOrWhiteSpace(search))
                {
                    var searchTerm = search.Trim();
                    dtos = dtos
                        .Where(x =>
                            (x.UserName != null && x.UserName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) ||
                            (x.FailureReason != null && x.FailureReason.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) ||
                            (x.IpAddress != null && x.IpAddress.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)))
                        .ToList();
                }

                return new PagedResult<LoginAttemptDto>
                {
                    Items = dtos.AsReadOnly(),
                    TotalCount = rawResult.TotalCount,
                    PageNumber = rawResult.PageNumber,
                    PageSize = pageSize
                };
            }
            catch (Exception)
            {
                // Return empty result on error instead of throwing
                return new PagedResult<LoginAttemptDto>
                {
                    Items = new List<LoginAttemptDto>().AsReadOnly(),
                    TotalCount = 0,
                    PageNumber = pagination.PageNumber,
                    PageSize = pagination.PageSize > 0 ? pagination.PageSize : 10
                };
            }
        }

        public async Task<PagedResult<LoginAttemptDto>> GetPagedByTenantAsync(PaginationParams pagination, string? tenantId, string? search = null)
        {
            try
            {
                var result = await _repository.GetPagedRawAsync(pagination);

                var dtos = result.Items
                    .Select(x => DeserializeLoginAttemptDto(x.Payload))
                    .Where(x => x != null)
                    .Cast<LoginAttemptDto>()
                    .Where(x => string.IsNullOrEmpty(tenantId) || x.TenantId == tenantId)
                    .Where(x => string.IsNullOrEmpty(search) ||
                        (x.UserName != null && x.UserName.Contains(search, StringComparison.OrdinalIgnoreCase)) ||
                        (x.FailureReason != null && x.FailureReason.Contains(search, StringComparison.OrdinalIgnoreCase)) ||
                        (x.IpAddress != null && x.IpAddress.Contains(search, StringComparison.OrdinalIgnoreCase)))
                    .ToList();

                return new PagedResult<LoginAttemptDto>
                {
                    Items = dtos.AsReadOnly(),
                    TotalCount = result.TotalCount,
                    PageNumber = result.PageNumber,
                    PageSize = result.PageSize
                };
            }
            catch (Exception)
            {
                return new PagedResult<LoginAttemptDto>
                {
                    Items = new List<LoginAttemptDto>().AsReadOnly(),
                    TotalCount = 0,
                    PageNumber = pagination.PageNumber,
                    PageSize = pagination.PageSize > 0 ? pagination.PageSize : 10
                };
            }
        }

        private LoginAttemptDto? DeserializeLoginAttemptDto(string? payload)
        {
            if (string.IsNullOrEmpty(payload))
                return null;

            try
            {
                return JsonSerializer.Deserialize<LoginAttemptDto>(payload);
            }
            catch
            {
                return null;
            }
        }
    }
}