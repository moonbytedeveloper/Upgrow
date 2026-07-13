using Microsoft.EntityFrameworkCore;
using VerifyIndia.Domain.Entities;
using VerifyIndia.Domain.IRepositories;

namespace VerifyIndia.Infrastructure.Repositories
{
    public class RegistrationApiLogRepository : IRegistrationApiLogRepository
    {
        private readonly AppDbContext _context;
        public RegistrationApiLogRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(RegistrationApiLog log)
        {
            await _context.RegistrationApiLog.AddAsync(log);
            await _context.SaveChangesAsync();
        }

        public Task<int> CountAsync(string apiPath, string mobileNo, int tenantId, DateTimeOffset sinceUtc)
        {
            if (string.IsNullOrWhiteSpace(apiPath) || string.IsNullOrWhiteSpace(mobileNo) || tenantId <= 0)
            {
                return Task.FromResult(0);
            }

            var normalizedApiPath = apiPath.Trim();
            var normalizedMobile = mobileNo.Trim();

            return _context.RegistrationApiLog
                .AsNoTracking()
                .CountAsync(log => log.ApiPath == normalizedApiPath
                    && log.MobileNo == normalizedMobile
                    && log.TenantId == tenantId
                    && log.CreatedAt >= sinceUtc);
        }
    }
}
