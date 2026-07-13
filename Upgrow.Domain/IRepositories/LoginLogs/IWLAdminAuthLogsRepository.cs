using System.Threading.Tasks;
using Upgrow.Domain.Entities.WL;

namespace Upgrow.Domain.IRepositories.LoginLogs
{
    public interface IWLAdminAuthLogsRepository
    {
        Task AddLogAsync(WL_AdminAuthLogs log);
        Task<byte[]?> GetLastCurrentHashAsync();
    }
}