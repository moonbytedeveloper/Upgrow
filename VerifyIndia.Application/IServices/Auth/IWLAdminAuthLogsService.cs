using System.Threading.Tasks;

namespace VerifyIndia.Application.IServices.Auth
{
    public interface IWLAdminAuthLogsService
    {
        Task LogAuthActionActivityAsync(string payload);
    }
}