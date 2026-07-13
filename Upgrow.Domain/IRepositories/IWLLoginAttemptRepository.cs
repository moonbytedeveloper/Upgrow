using Upgrow.Domain.Common;
using Upgrow.Domain.Entities;

namespace Upgrow.Domain.IRepositories
{
    public interface IWLLoginAttemptRepository
    {
        Task AddAsync(WL_LoginAttempts attempt);
        Task<byte[]?> GetLastCurrentHashAsync();
        Task<PagedResult<WL_LoginAttempts>> GetPagedRawAsync(PaginationParams pagination);
    }
}