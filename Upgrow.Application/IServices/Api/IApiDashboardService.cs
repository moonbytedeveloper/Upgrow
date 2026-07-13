using System.Threading.Tasks;
using Upgrow.Application.DTO.AIX;

namespace Upgrow.Application.IServices.Api
{
    public interface IApiDashboardService
    {
         
        Task<DashboardContentDto> GetDashboardContentAsync(string xCategoryUuid, string versionUuid);
    }
}