using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections.Concurrent;
using System.Diagnostics.Metrics;
using System.Timers;
using VerifyIndia.Application.DTO.CustomerPanel;
using VerifyIndia.Application.DTO.CustomerPanel.QueryResults;
using VerifyIndia.Application.Interfaces;
using VerifyIndia.Domain.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace VerifyIndia.Infrastructure.Repositories.CustomerPanel
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly AppDbContext _context;

        public DashboardRepository(AppDbContext context)
        {
            _context = context;
        }

        //public async Task<List<ApiQueryResult>>
        //GetVerificationApisByCategoryAsync(
        //    string categoryUuid,
        //    string customerUuid)
        //{
        //    return await _context
        //        .Set<ApiQueryResult>()
        //        .FromSqlInterpolated(
        //            $@"EXEC sp_GetVerificationApisByCategory
        //                @CategoryUUID={categoryUuid},
        //                @CustomerUUID={customerUuid}")
        //        .AsNoTracking()
        //        .ToListAsync();

        //}

        public async Task<List<ApiQueryResult>> GetVerificationApisByCategoryAsync(
    string categoryUuid,
    string customerUuid)
        {
            return await _context
                .Set<ApiQueryResult>()
                .FromSqlInterpolated($@"
            EXEC sp_GetVerificationApisByCategory_WithCartDetails
                @CategoryUUID = {categoryUuid},
                @CustomerUUID = {customerUuid}")
                .AsNoTracking()
                .ToListAsync();
        }


        public async Task<List<ApiSectionResultDto>>
        GetApiSectionsAsync(
            string categoryUuid)
        {

            try
            {
                return await _context
                        .Set<ApiSectionResultDto>()
                        .FromSqlInterpolated(
                            $@"EXEC sp_GetApiSections
                        @CategoryUUID={categoryUuid}")
                        .AsNoTracking()
                        .ToListAsync();
            }
            catch (Exception ex)
            {

                throw;
            }

            #region Store Procedure for reference
            /*CREATE OR ALTER PROCEDURE sp_GetApiSections
(
    @CategoryUUID NVARCHAR(50)
)
AS
BEGIN
    SET NOCOUNT ON;

            SELECT
        api.UUID AS ApiUUID,

        sec.UUID AS SectionUUID,
        sec.Title AS SectionTitle,
        sec.Description AS SectionDescription,
        sec.Sequence AS SectionSequence,

        fld.Title AS FieldTitle,
        fld.Sequence AS FieldSequence

    FROM Master_Api api

    INNER JOIN ApiInfoSection sec
        ON sec.ApiUUID = api.UUID
       AND sec.IsActive = 1

    LEFT JOIN ApiInfoFields fld
        ON fld.InfoSectionUUID = sec.UUID
       AND fld.IsActive = 1

    WHERE api.ApiCategoryUUID = @CategoryUUID
      AND api.IsActive = 1

    ORDER BY
        api.DisplayOrder,
        sec.Sequence,
        fld.Sequence;
            END
            GO*/

            #endregion
        }

        public async Task<List<ApiCategoryDto>> GetActiveCategoriesAsync()
        {
            // Retrieve all the active Api_Category with counting of related active Master_Api
            return await _context.Api_Category
                .Where(c => c.IsActive)
                .Select(c => new ApiCategoryDto
                {
                    UUID = c.UUID,
                    CategoryName = c.CategoryName,
                    SequenceNo = c.SequenceNo,
                    Icon = c.Icon,
                    ApiCount = _context.Master_Api.Count(api => api.ApiCategoryUUID == c.UUID && api.IsActive)
                })
                .ToListAsync();
        }

        public async Task<List<CategoryApiQueryResult>>
    GetAllCategoriesWithApisAsync(string customerUuid)
        {
            return await _context
                .Set<CategoryApiQueryResult>()
                .FromSqlInterpolated($@"
            EXEC sp_GetAllCategoriesWithApis
                @CustomerUUID = {customerUuid}")
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<string?> GetCartByCustomerAsync(string customerUuid)
        {
            var cart = await _context
                .Master_Carts
                .FirstOrDefaultAsync(c => c.VerifierUUID == customerUuid && c.IsActive == true);

            return cart?.UUID;
        }

        public async Task<string?> GetReqPayload(string cartUuid, string apiUuid)
        {
            var reqPayload = await _context
                .CartDetails
                .FirstOrDefaultAsync(r => r.CartUUID == cartUuid && r.ApiUUID == apiUuid && r.IsActive == true);
            return reqPayload?.ReqPayload;
        }


    }
}