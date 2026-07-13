using System.Threading.Tasks;
using VerifyIndia.Domain.Entities;

namespace VerifyIndia.Domain.IRepositories
{
    public interface IAdminAuthLogsRepository
    {
        Task AddAsync(AdminAuthLogs log);
        Task <byte[]?> GetLastCurrentHashAsync();
    }
}