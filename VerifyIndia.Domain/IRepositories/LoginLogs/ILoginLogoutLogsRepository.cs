using VerifyIndia.Domain.Common;
using VerifyIndia.Domain.Entities;

namespace VerifyIndia.Domain.IRepositories.LoginLogs
{
    public interface ILoginLogoutLogsRepository
    {
        Task<PagedResult<AdminAuthLogs>> GetPagedRawAsync(PaginationParams pagination);
    }
}