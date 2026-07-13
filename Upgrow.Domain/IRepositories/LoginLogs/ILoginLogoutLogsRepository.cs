using Upgrow.Domain.Common;
using Upgrow.Domain.Entities;

namespace Upgrow.Domain.IRepositories.LoginLogs
{
    public interface ILoginLogoutLogsRepository
    {
        Task<PagedResult<AdminAuthLogs>> GetPagedRawAsync(PaginationParams pagination);
    }
}