using System.Threading.Tasks;
using Upgrow.Domain.Entities;

namespace Upgrow.Domain.IRepositories
{
    public interface IAdminAuthLogsRepository
    {
        Task AddAsync(AdminAuthLogs log);
        Task <byte[]?> GetLastCurrentHashAsync();
    }
}