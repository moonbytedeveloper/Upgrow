using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Razorpay.Api;
using System.Text.Json;
using Upgrow.Application.Commands;
using Upgrow.Application.DTO.CustomerPanel;
using Upgrow.Application.Interfaces;
using Upgrow.Application.IServices;
using Upgrow.Application.IServices.CustomerPanel;
using Upgrow.Domain.Common;
using Upgrow.Domain.Entities;
using Upgrow.Domain.IRepositories;

namespace Upgrow.Application.Services.CustomerPanel
{
    public class DashboardService : IDashboardService
    {
        private readonly ICommonRepository _commonRepository;
        private readonly IDashboardRepository _dashboardRepository;
        private readonly IMasterRepository<Api_Category> _categoryRepo;
        private readonly IMasterRepository<Master_Api> _apiRepo;
        private readonly IMasterRepository<Pinned_Services> _pinnedRepo;

        public DashboardService(
            IDashboardRepository dashboardRepository,
            ICommonRepository commonRepository,
            IMasterRepository<Api_Category> categoryRepo,
            IMasterRepository<Master_Api> apiRepo,
            IMasterRepository<Pinned_Services> pinnedRepo)
        {
            _dashboardRepository = dashboardRepository;
            _commonRepository = commonRepository;
            _categoryRepo = categoryRepo;
            _apiRepo = apiRepo;
            _pinnedRepo = pinnedRepo;
        }


        public async Task<List<ApiCategoryDto>> GetActiveCategoriesAsync()
        {
            return await _dashboardRepository.GetActiveCategoriesAsync();
        }

        public async Task<bool> IsCategoryExists(string Uuid)
        {
            return await _categoryRepo.ExistsAsync(x => x.UUID == Uuid);

        }

        public async Task<bool> IsApiExists(string Uuid)
        {
            return await _apiRepo.ExistsAsync(x => x.UUID == Uuid);
        }

        public async Task<int> ChangeCurrentStep(string mobileNumber, string code)
        {
            return await _commonRepository.ChangeCurrentStep(mobileNumber, code);
        }

        public async Task<bool> AddPinnedStatusByApi(
        string apiUuid,
        string customerUuid)
        {

            try
            {
                var existingPinned = await _pinnedRepo
                        .FindAllAsync(x =>
                            x.ApiUUID == apiUuid &&
                            x.CustomerUUID == customerUuid);


                var existingPinnedEntity = existingPinned.FirstOrDefault();

                if (existingPinnedEntity == null)
                {
                    await _pinnedRepo.AddAsync(new Pinned_Services
                    {
                        UUID = Utils.GetUUID(),
                        ApiUUID = apiUuid,
                        CustomerUUID = customerUuid,
                        IsActive = true
                    });

                    return true;
                }

                existingPinnedEntity.IsActive = !existingPinnedEntity.IsActive;

                await _pinnedRepo.UpdateAsync(existingPinnedEntity);

                return existingPinnedEntity.IsActive;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<ApiDto>> GetVerificationApisbyCategory(
    string categoryUuid,
    string customerUuid)
        {
            var categoryExists = await _categoryRepo.ExistsAsync(x =>
                x.UUID == categoryUuid &&
                x.IsActive);

            if (!categoryExists)
                throw new Exception("Category not found.");

            /*var customerExists = await _tenantRepo.ExistsAsync(x =>
                x.UUID == customerUuid &&
                x.IsActive);

            if (!customerExists)
                throw new Exception("Customer not found.");*/

            var apiResults = await _dashboardRepository
                .GetVerificationApisByCategoryAsync(
                    categoryUuid,
                    customerUuid);

            if (!apiResults.Any())
                return new List<ApiDto>();

            var sectionRows = await _dashboardRepository
                .GetApiSectionsAsync(
                    categoryUuid);

            var apis = apiResults
                .Select(x => new ApiDto
                {
                    UUID = x.UUID,
                    ApiName = x.ApiName,
                    Code = x.Code,
                    ShortDescription = x.ShortDescription,
                    DisplayOrder = x.DisplayOrder,

                    IsPinned = x.IsPinned,
                    IsAddedInCart = x.IsAddedInCart,

                    Price = x.Price,

                    IsMultiEndpoint = x.IsMultiEndpoint,

                    ComponentName = x.ComponentName,
                    SubmitUrl = x.SubmitUrl,
                    SubmitHttpMethod = x.SubmitHttpMethod,

                    SecondComponentName = x.SecondComponentName,
                    SecondSubmitUrl = x.SecondSubmitUrl,
                    SecondSubmitHttpMethod = x.SecondSubmitHttpMethod,

                    IsConsentRequired = x.IsConsentRequired,
                    RequestFields = BuildRequestFields(x.ReqPayload)
                })
                .ToList();

            var sectionLookup = sectionRows
                .GroupBy(x => x.ApiUUID)
                .ToDictionary(
                    x => x.Key,
                    x => x.GroupBy(y => y.SectionUUID)
                        .Select(sectionGroup =>
                            new ApiSectionDto
                            {
                                Title = sectionGroup
                                    .First()
                                    .SectionTitle,

                                Description = sectionGroup
                                    .First()
                                    .SectionDescription,

                                Sequence = sectionGroup
                                    .First()
                                    .SectionSequence,

                                Fields = sectionGroup
                                    .Where(f =>
                                        !string.IsNullOrWhiteSpace(
                                            f.FieldTitle))
                                    .OrderBy(f =>
                                        f.FieldSequence)
                                    .Select(f =>
                                        new ApiFieldDto
                                        {
                                            Title =
                                                f.FieldTitle,

                                            Sequence =
                                                f.FieldSequence ?? 0
                                        })
                                    .ToList()
                            })
                        .OrderBy(s => s.Sequence)
                        .ToList());

            foreach (var api in apis)
            {
                if (sectionLookup.TryGetValue(
                        api.UUID,
                        out var sections))
                {
                    api.Sections = sections;
                }
            }

            return apis;
        }

        private static List<ApiRequestFieldDto> BuildRequestFields(string? reqPayload)
        {
            if (string.IsNullOrWhiteSpace(reqPayload))
                return new();

            reqPayload = reqPayload.Trim();

            // Fix invalid JSON stored without braces
            if (!reqPayload.StartsWith("{"))
            {
                reqPayload = "{" + reqPayload + "}";
            }

            var data = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(reqPayload);

            return data?
                .Select(x => new ApiRequestFieldDto
                {
                    Name = x.Key,
                    Value = x.Value.ToString()
                })
                .ToList()
                ?? new();
        }

        public async Task<List<CategoryWithApiDto>>
    GetCategoriesWithApisAsync(string customerUuid)
        {
            var records = await _dashboardRepository
                .GetAllCategoriesWithApisAsync(customerUuid);

            return records
                .GroupBy(x => new
                {
                    x.CategoryUUID,
                    x.CategoryName,
                    x.SequenceNo,
                    x.Icon
                })
                .OrderBy(x => x.Key.SequenceNo)
                .Select(g => new CategoryWithApiDto
                {
                    UUID = g.Key.CategoryUUID,
                    CategoryName = g.Key.CategoryName,
                    SequenceNo = g.Key.SequenceNo,
                    Icon = g.Key.Icon,
                    ApiCount = g.Count(),

                    Apis = g
                        .OrderBy(x => x.DisplayOrder)
                        .Select(x => new VerificationApiDto
                        {
                            UUID = x.ApiUUID,
                            ApiName = x.ApiName,
                            Code = x.Code,
                            ShortDescription = x.ShortDescription,
                            DisplayOrder = x.DisplayOrder,

                            IsPinned = x.IsPinned,
                            IsAddedInCart = x.IsAddedInCart,

                            Price = x.Price,

                            IsMultiEndpoint = x.IsMultiEndpoint,

                            ComponentName = x.ComponentName,
                            SubmitUrl = x.SubmitUrl,
                            SubmitHttpMethod = x.SubmitHttpMethod,

                            SecondComponentName = x.SecondComponentName,
                            SecondSubmitUrl = x.SecondSubmitUrl,
                            SecondSubmitHttpMethod = x.SecondSubmitHttpMethod,

                            IsConsentRequired = x.IsConsentRequired
                        })
                        .ToList()
                })
                .ToList();
        }

        public async Task<ApiReadMoreDto?> GetApiDetailsAsync(
    string apiUuid,
    string customerUuid)
        {
            if (string.IsNullOrWhiteSpace(apiUuid))
                return null;

            var apiEntity = await _apiRepo.GetByUuidAsync(apiUuid);

            if (apiEntity == null || !apiEntity.IsActive)
                return null;

            var dto = new ApiReadMoreDto();

            try
            {
                var sectionRows = await _dashboardRepository
                    .GetApiSectionsAsync(apiEntity.ApiCategoryUUID);

                dto.Sections = sectionRows
                    .Where(r => r.ApiUUID == apiUuid)
                    .GroupBy(y => y.SectionUUID)
                    .Select(sectionGroup => new ApiSectionDto
                    {
                        Title = sectionGroup.First().SectionTitle,
                        Description = sectionGroup.First().SectionDescription,
                        Sequence = sectionGroup.First().SectionSequence,

                        Fields = sectionGroup
                            .Where(f => !string.IsNullOrWhiteSpace(f.FieldTitle))
                            .OrderBy(f => f.FieldSequence)
                            .Select(f => new ApiFieldDto
                            {
                                Title = f.FieldTitle,
                                Sequence = f.FieldSequence ?? 0
                            })
                            .ToList()
                    })
                    .OrderBy(s => s.Sequence)
                    .ToList();
            }
            catch
            {
                dto.Sections = new List<ApiSectionDto>();
            }

            return dto;
        }


        public async Task<string?> GetCartUuidByCustomer(string customerUuid)
        {
            var cart = await _dashboardRepository.GetCartByCustomerAsync(customerUuid);

            if (string.IsNullOrWhiteSpace(cart))
                return null;

            return cart;
        }

        public async Task<string?> GetReqPayloadByCustomerAndApi(string cartUuid, string apiUuid)
        {
            var reqpayload = await _dashboardRepository.GetReqPayload(cartUuid, apiUuid);
            if (string.IsNullOrWhiteSpace(reqpayload))
                return null;

            return reqpayload;
        }


    }
}