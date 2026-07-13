using System.Threading.Tasks;
using Upgrow.Domain.Common;
using Upgrow.Domain.Entities;

namespace Upgrow.Domain.IRepositories
{
    public interface ILoginAttemptRepository
    {
        Task AddAsync(LoginAttempts attempt);
        Task<byte[]?> GetLastCurrentHashAsync();
        Task<PagedResult<LoginAttempts>> GetPagedRawAsync(PaginationParams pagination);

    }
}