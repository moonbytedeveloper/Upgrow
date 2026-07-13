using System.Threading.Tasks;
using VerifyIndia.Application.DTO.AIX;

namespace VerifyIndia.Application.IServices.Api
{
    public interface IApiDashboardService
    {
         
        Task<DashboardContentDto> GetDashboardContentAsync(string xCategoryUuid, string versionUuid);
    }
}