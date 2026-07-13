using System.Threading.Tasks;
using VerifyIndia.Domain.Entities.WL;

namespace VerifyIndia.Domain.IRepositories.LoginLogs
{
    public interface IWLAdminAuthLogsRepository
    {
        Task AddLogAsync(WL_AdminAuthLogs log);
        Task<byte[]?> GetLastCurrentHashAsync();
    }
}