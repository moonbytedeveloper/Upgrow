using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Commands.Master;
using VerifyIndia.Application.DTO.Master;
using VerifyIndia.Application.DTOs;
using VerifyIndia.Application.IServices.Master;
using VerifyIndia.Domain.Common;
using VerifyIndia.Domain.Entities;
using VerifyIndia.Domain.IRepositories;

namespace VerifyIndia.Application.Services.Master
{
    public class WLMasterCityService : MasterServiceBase<WL_MasterCity, WLMasterCityDto, WLMasterCityCommand>, IWLMasterCityService
    {
        private readonly IMasterRepository<WL_MasterCountry> _countryRepository;
        private readonly IMasterRepository<WL_MasterState> _stateRepository;
        public WLMasterCityService(IMasterRepository<WL_MasterCity> repository, IMapper mapper,
             IMasterRepository<WL_MasterCountry> countryRepository,
             IMasterRepository<WL_MasterState> stateRepository)
          : base(repository, mapper)
        {
            _countryRepository = countryRepository;
            _stateRepository = stateRepository;
        }

        //public override async Task<PagedResult<MasterCityDto>> GetPagedAsync(DataTableRequest request)

        //{
        //    // prepare search term (lower-cased) and optional country/state matches
        //    var search = request.Search?.Trim();
        //    string? searchLower = string.IsNullOrWhiteSpace(search) ? null : search!.ToLower();

        //    // Fetch countries and states once (we need them later to populate CountryName/StateName;
        //    // also use them to find matches by title)
        //    var allCountries = await _countryRepository.GetAllActiveAsync();
        //    var allStates = await _stateRepository.GetAllActiveAsync();

        //    // If there's a search term and some countries match it by title, collect their UUIDs
        //    List<string> matchingCountryUuids = new();
        //    if (searchLower != null)
        //    {
        //        matchingCountryUuids = allCountries
        //            .Where(c => !string.IsNullOrEmpty(c.Title) && c.Title!.ToLower().Contains(searchLower))
        //            .Select(c => c.UUID!)
        //            .ToList();
        //    }

        //    // If there's a search term and some states match it by title, collect their UUIDs
        //    List<string> matchingStateUuids = new();
        //    if (searchLower != null)
        //    {
        //        matchingStateUuids = allStates
        //            .Where(s => !string.IsNullOrEmpty(s.Title) && s.Title!.ToLower().Contains(searchLower))
        //            .Select(s => s.UUID!)
        //            .ToList();
        //    }

        //    // Build city filter: title/shorttitle OR city.CountryUUID in matchingCountryUuids OR city.StateUUID in matchingStateUuids
        //    Expression<Func<Master_City, bool>>? filter = null;
        //    if (searchLower != null)
        //    {
        //        filter = x =>
        //            (x.Title != null && x.Title.ToLower().Contains(searchLower)) ||
        //            (x.ShortTitle != null && x.ShortTitle.ToLower().Contains(searchLower)) ||
        //            (matchingCountryUuids.Count > 0 && matchingCountryUuids.Contains(x.CountryUUID)) ||
        //            (matchingStateUuids.Count > 0 && matchingStateUuids.Contains(x.StateUUID));
        //    }

        //    // Sorting as per existing logic
        //    var orderBy = BuildSortExpression(request.SortColumn, request.SortDirection);

        //    // Run paged query against state repository with the composed filter
        //    // Repository expects a PaginationParams object (DataTableRequest inherits it)
        //    var pagedResult = await _repository.GetPagedAsync(
        //        filter,
        //        request,
        //        orderBy);

        //    var entities = pagedResult.Items;
        //    // map to MasterCityDto (was incorrectly mapping to MasterStateDto)
        //    var data = _mapper.Map<List<MasterCityDto>>(entities);

        //    // Populate country names from the country list we already fetched
        //    var countryUuids = data
        //        .Where(x => !string.IsNullOrEmpty(x.CountryUUID))
        //        .Select(x => x.CountryUUID!)
        //        .Distinct()
        //        .ToList();

        //    if (countryUuids.Any())
        //    {
        //        var countryMap = allCountries
        //            .Where(c => countryUuids.Contains(c.UUID))
        //            .ToDictionary(c => c.UUID!, c => c.Title);

        //        foreach (var dto in data)
        //        {
        //            if (!string.IsNullOrEmpty(dto.CountryUUID) && countryMap.ContainsKey(dto.CountryUUID))
        //            {
        //                dto.CountryName = countryMap[dto.CountryUUID];
        //            }
        //        }
        //    }

        //    // Populate state names from the state list we already fetched
        //    var stateUuids = data
        //        .Where(x => !string.IsNullOrEmpty(x.StateUUID))
        //        .Select(x => x.StateUUID!)
        //        .Distinct()
        //        .ToList();

        //    if (stateUuids.Any())
        //    {
        //        var stateMap = allStates
        //            .Where(s => stateUuids.Contains(s.UUID))
        //            .ToDictionary(s => s.UUID!, s => s.Title);

        //        foreach (var dto in data)
        //        {
        //            if (!string.IsNullOrEmpty(dto.StateUUID) && stateMap.ContainsKey(dto.StateUUID))
        //            {
        //                dto.StateName = stateMap[dto.StateUUID];
        //            }
        //        }
        //    }
        //    return new PagedResult<MasterCityDto>
        //    {
        //        Items = data,
        //        TotalCount = pagedResult.TotalCount,
        //        PageNumber = pagedResult.PageNumber,
        //        PageSize = pagedResult.PageSize
        //    };
        //    // return (total, data);
        //}

        // Define which field to check for duplicates
        protected override async Task<bool> IsDuplicateAsync(WLMasterCityCommand command)
        {
            var title = command.Title?.ToLower().Trim() ?? string.Empty;
            var country = command.CountryUUID ?? string.Empty;
            var state = command.StateUUID ?? string.Empty;

            //return await _repository.ExistsAsync(x =>
            //    x.Title!.ToLower().Trim() == command.Title.ToLower().Trim() &&
            //    x.UUID != command.UUID);
            return await _repository.ExistsAsync(x =>
               x.Title != null && x.Title.ToLower().Trim() == title &&
               (x.CountryUUID ?? string.Empty) == country &&
               (x.StateUUID ?? string.Empty) == state &&
               x.UUID != command.UUID);
        }

        // Define which fields to search
        protected override Expression<Func<WL_MasterCity, bool>>? BuildSearchFilter(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();

            return x =>
                (x.Title != null && x.Title.ToLower().Contains(searchTerm)) ||
                (x.ShortTitle != null && x.ShortTitle.ToLower().Contains(searchTerm)) ||
                (x.StateUUID != null && x.StateUUID.ToLower().Contains(searchTerm)) ||
                (x.CountryUUID != null && x.CountryUUID.ToLower().Contains(searchTerm));  // Added search on country name
        }

        // Define sorting (optional - remove if default Id sorting is fine)
        protected override Func<IQueryable<WL_MasterCity>, IOrderedQueryable<WL_MasterCity>>? BuildSortExpression(
            string? sortColumn,
            string? sortDirection)
        {
            var isAsc = sortDirection?.ToLower() == "asc";

            return sortColumn?.ToLower() switch
            {
                "name" => q => isAsc ? q.OrderBy(x => x.Title) : q.OrderByDescending(x => x.Title),
                "shortname" => q => isAsc ? q.OrderBy(x => x.ShortTitle) : q.OrderByDescending(x => x.ShortTitle),
                "country" => q => isAsc ? q.OrderBy(x => x.CountryUUID) : q.OrderByDescending(x => x.CountryUUID),
                "state" => q => isAsc ? q.OrderBy(x => x.StateUUID) : q.OrderByDescending(x => x.StateUUID),
                _ => q => isAsc ? q.OrderBy(x => x.Id) : q.OrderByDescending(x => x.Id)
            };
        }

    }
}

