using System.Text.Json;
using Upgrow.Application.DTO.Auth;
using Upgrow.Application.IServices.Auth;
using Upgrow.Domain.Entities;
using Upgrow.Domain.IRepositories;

namespace Upgrow.Application.Services.Auth
{
    public class RegistrationApiLogService : IRegistrationApiLogService
    {
        private readonly IRegistrationApiLogRepository _repository;

        public RegistrationApiLogService(IRegistrationApiLogRepository repository)
        {
            _repository = repository;
        }

        public async Task AddAsync(RegistrationApiLogDto dto)
        {
            var entity = new RegistrationApiLog
            {
                UUID = dto.UUID ?? Guid.NewGuid().ToString(),
                CustomerUUID = dto.CustomerUUID,
                MobileNo = dto.MobileNo,
                TenantId = dto.TenantId,
                ApiPath = dto.ApiPath,
                HttpMethod = dto.HttpMethod,
                ApiRequest = dto.ApiRequest,
                ApiResponse = dto.ApiResponse,
                StatusCode = dto.StatusCode,
                CreatedAt = dto.CreatedAt,
                IpAddress = dto.IpAddress
            };

            await _repository.AddAsync(entity);
        }

        public Task<int> CountAsync(string apiPath, string mobileNo, int tenantId, DateTimeOffset sinceUtc)
            => _repository.CountAsync(apiPath, mobileNo, tenantId, sinceUtc);
    }
}
