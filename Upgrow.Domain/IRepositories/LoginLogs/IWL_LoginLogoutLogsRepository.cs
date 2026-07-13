using Upgrow.Domain.Common;
using Upgrow.Domain.Entities.WL;

namespace Upgrow.Domain.IRepositories.LoginLogs
{
    public interface IWL_LoginLogoutLogsRepository
    {
        Task<PagedResult<WL_AdminAuthLogs>> GetPagedRawAsync(PaginationParams request);
    }
}