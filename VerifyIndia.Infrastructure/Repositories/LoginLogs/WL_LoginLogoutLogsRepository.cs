using Microsoft.EntityFrameworkCore;
using VerifyIndia.Domain.Common;
using VerifyIndia.Domain.Entities.WL;
using VerifyIndia.Domain.IRepositories.LoginLogs;

namespace VerifyIndia.Infrastructure.Repositories.LoginLogs
{
    public class WL_LoginLogoutLogsRepository : IWL_LoginLogoutLogsRepository
    {
        private readonly AppDbContext _context;

        public WL_LoginLogoutLogsRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<WL_AdminAuthLogs>> GetPagedRawAsync(PaginationParams request)
        {
            var query = _context.WL_AdminAuthLogs.AsNoTracking().OrderByDescending(x => x.Id);

            var totalCount = await query.CountAsync();
            var items = await query.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize).ToListAsync();

            return new PagedResult<WL_AdminAuthLogs>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };
        }
    }
}