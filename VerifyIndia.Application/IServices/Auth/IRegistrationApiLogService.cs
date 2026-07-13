using Upgrow.Application.DTO.Auth;

namespace Upgrow.Application.IServices.Auth
{
    public interface IRegistrationApiLogService
    {
        Task AddAsync(RegistrationApiLogDto dto);
        Task<int> CountAsync(string apiPath, string mobileNo, int tenantId, DateTimeOffset sinceUtc);
    }
}
