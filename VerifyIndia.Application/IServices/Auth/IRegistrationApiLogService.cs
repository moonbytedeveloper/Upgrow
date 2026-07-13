using VerifyIndia.Application.DTO.Auth;

namespace VerifyIndia.Application.IServices.Auth
{
    public interface IRegistrationApiLogService
    {
        Task AddAsync(RegistrationApiLogDto dto);
        Task<int> CountAsync(string apiPath, string mobileNo, int tenantId, DateTimeOffset sinceUtc);
    }
}
