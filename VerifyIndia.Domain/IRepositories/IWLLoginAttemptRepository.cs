using VerifyIndia.Domain.Common;
using VerifyIndia.Domain.Entities;

namespace VerifyIndia.Domain.IRepositories
{
    public interface IWLLoginAttemptRepository
    {
        Task AddAsync(WL_LoginAttempts attempt);
        Task<byte[]?> GetLastCurrentHashAsync();
        Task<PagedResult<WL_LoginAttempts>> GetPagedRawAsync(PaginationParams pagination);
    }
}