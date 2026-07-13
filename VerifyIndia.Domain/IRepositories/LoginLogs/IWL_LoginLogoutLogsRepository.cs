using VerifyIndia.Domain.Common;
using VerifyIndia.Domain.Entities.WL;

namespace VerifyIndia.Domain.IRepositories.LoginLogs
{
    public interface IWL_LoginLogoutLogsRepository
    {
        Task<PagedResult<WL_AdminAuthLogs>> GetPagedRawAsync(PaginationParams request);
    }
}