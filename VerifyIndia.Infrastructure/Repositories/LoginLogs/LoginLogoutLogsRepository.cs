using Microsoft.EntityFrameworkCore;
using VerifyIndia.Domain.Common;
using VerifyIndia.Domain.Entities;
using VerifyIndia.Domain.IRepositories.LoginLogs;
using VerifyIndia.Infrastructure.Extensions;

namespace VerifyIndia.Infrastructure.Repositories.LoginLogs
{
    public sealed class LoginLogoutLogsRepository : ILoginLogoutLogsRepository
    {
        private readonly AppDbContext _context;

        public LoginLogoutLogsRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<AdminAuthLogs>> GetPagedRawAsync(PaginationParams pagination)
        {
            var query = _context.AdminAuthLogs
                .AsNoTracking()
                .OrderByDescending(x => x.Id);

            // Uses your existing paging extension (safe because entity has no non-null master fields)
            return await query.ToPagedResultAsync(pagination);
        }
    }
}