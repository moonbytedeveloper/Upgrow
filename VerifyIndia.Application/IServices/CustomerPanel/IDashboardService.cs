using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.DTO.CustomerPanel;

namespace Upgrow.Application.IServices.CustomerPanel
{
    public interface IDashboardService
    {
        Task<List<ApiCategoryDto>> GetActiveCategoriesAsync();
        Task<bool> IsCategoryExists(string Uuid);

        Task<bool> IsApiExists(string Uuid);
        Task<bool> AddPinnedStatusByApi(string apiUuid, string customerUuid);

        Task<List<ApiDto>> GetVerificationApisbyCategory(string categoryUuid, string customerUuid);

        Task<int> ChangeCurrentStep(string mobileNumber, string code);

        Task<List<CategoryWithApiDto>> GetCategoriesWithApisAsync(
    string customerUuid);

        Task<ApiReadMoreDto?> GetApiDetailsAsync(string apiUuid, string customerUuid);
        Task<string?> GetCartUuidByCustomer(string customerUuid);

        Task<string?> GetReqPayloadByCustomerAndApi(string cartUuid, string apiUuid);
    }
}
