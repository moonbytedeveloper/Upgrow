using Microsoft.EntityFrameworkCore;
using Upgrow.Domain.Common;
using Upgrow.Domain.Entities;
using Upgrow.Domain.IRepositories.LoginLogs;
using Upgrow.Infrastructure.Extensions;

namespace Upgrow.Infrastructure.Repositories.LoginLogs
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