using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.DTO.CustomerPanel;
using VerifyIndia.Application.DTO.CustomerPanel.QueryResults;

namespace VerifyIndia.Application.Interfaces
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
