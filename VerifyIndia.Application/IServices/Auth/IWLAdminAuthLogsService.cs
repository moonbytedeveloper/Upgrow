using System.Threading.Tasks;

namespace Upgrow.Application.IServices.Auth
{
    public interface IWLAdminAuthLogsService
    {
        Task LogAuthActionActivityAsync(string payload);
    }
}