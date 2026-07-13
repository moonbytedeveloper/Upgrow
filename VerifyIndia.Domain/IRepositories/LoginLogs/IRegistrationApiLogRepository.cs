using VerifyIndia.Domain.Entities;

namespace VerifyIndia.Domain.IRepositories
{
    public interface IRegistrationApiLogRepository
    {
        Task AddAsync(RegistrationApiLog log);
        Task<int> CountAsync(string apiPath, string mobileNo, int tenantId, DateTimeOffset sinceUtc);
    }
}
