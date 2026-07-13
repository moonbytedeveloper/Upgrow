using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.DTO.CustomerPanel;
using Upgrow.Application.DTO.CustomerPanel.QueryResults;

namespace Upgrow.Application.Interfaces
{
    public interface IDashboardRepository
    {
        Task<List<ApiQueryResult>> GetVerificationApisByCategoryAsync(string categoryUuid, string customerUuid);
        Task<List<ApiSectionResultDto>> GetApiSectionsAsync(string categoryUuid);

        Task<List<ApiCategoryDto>> GetActiveCategoriesAsync();

        Task<List<CategoryApiQueryResult>> GetAllCategoriesWithApisAsync(string customerUuid);

        Task<string?> GetCartByCustomerAsync(string customerUuid);

        Task<string?> GetReqPayload(string cartUuid, string apiUuid);


    }
}
