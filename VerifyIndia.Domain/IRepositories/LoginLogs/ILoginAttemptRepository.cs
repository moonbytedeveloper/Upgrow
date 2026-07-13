using System.Threading.Tasks;
using VerifyIndia.Domain.Common;
using VerifyIndia.Domain.Entities;

namespace VerifyIndia.Domain.IRepositories
{
    public interface ILoginAttemptRepository
    {
        Task AddAsync(LoginAttempts attempt);
        Task<byte[]?> GetLastCurrentHashAsync();
        Task<PagedResult<LoginAttempts>> GetPagedRawAsync(PaginationParams pagination);

    }
}