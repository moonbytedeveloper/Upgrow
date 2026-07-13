using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Commands.Master;
using VerifyIndia.Application.DTO.DropDown;
using VerifyIndia.Application.DTOs;
using VerifyIndia.Application.IServices.Master;
using VerifyIndia.Application.IServices.WL;
using VerifyIndia.Application.Services.Master;
using VerifyIndia.Domain.Common;
using VerifyIndia.Domain.Entities;
using VerifyIndia.Domain.IRepositories;

namespace VerifyIndia.Application.Services.WL
{
    public abstract class WLBaseService<TEntity, TDto, TCommand>
     : MasterServiceBase<TEntity, TDto, TCommand>,
      IWLBaseService<TEntity, TDto, TCommand> 
     where TEntity : TenantEntity, new()
     where TDto : class
     where TCommand : class, IMasterCommand
    {
        protected readonly IMasterRepository<Tenant> _tenantRepository;

        protected WLBaseService(
       IMasterRepository<TEntity> repository,
       IMasterRepository<Tenant> tenantRepository,
       IMapper mapper)
       : base(repository, mapper)
        {
            _tenantRepository = tenantRepository;
        }

        #region Common Tenant Methods

        protected async Task<int?> ResolveTenantIdAsync(string? tenantUuid)
        {
            if (string.IsNullOrWhiteSpace(tenantUuid))
                return null;

            var tenants = await _tenantRepository.GetAllActiveAsync();
            var tenant = tenants.FirstOrDefault(t => t.UUID == tenantUuid.Trim());

            return tenant != null ? (int)tenant.Id : -1;
        }

        protected async Task<List<int>> GetTenantIdsBySearchAsync(string? search)
        {
            if (string.IsNullOrWhiteSpace(search))
                return new List<int>();

            var tenants = await _tenantRepository.GetAllActiveAsync();

            return tenants
                .Where(t => !string.IsNullOrWhiteSpace(t.TenantName) &&
                            t.TenantName.ToLower().Contains(search))
                .Select(t => (int)t.Id)
                .ToList();
        }

        //protected async Task<TEntity?> GetByUuidIgnoringTenantFilterAsync(string uuid)
        //{
        //    var result = await _repository.GetPagedAsync(
        //        x => x.UUID == uuid,
        //        new PaginationParams { PageNumber = 1, PageSize = 1 },
        //        null,
        //        q => q.IgnoreQueryFilters());

        //    return result.Items.FirstOrDefault();
        //}

        public virtual async Task<List<MasterDropDownDto>> GetTenantDropdownAsync()
        {
            var tenants = await _tenantRepository.GetAllActiveAsync();

            return tenants
                .Where(t => t.IsPlatformOwner == false && t.IsActive == true)
                .OrderBy(t => t.TenantName)
                .Select(t => new MasterDropDownDto
                {
                    UUID = t.UUID,
                    Title = t.TenantName
                })
                .ToList();
        }
        public async Task<PagedResult<TDto>> GetPagedByTenantAsync(DataTableRequest request, string? tenantUuid)
        {
            var searchForFilter = request.Search?.Trim();
            return await GetPagedWithTenantAsync(
                request,
                tenantUuid,
                BuildSearchFilter(searchForFilter),
                entity => _mapper.Map<TDto>(entity)); // works for any TEntity -> TDto
        }
        #endregion

        #region Pagination with Tenant and Search

        public virtual async Task<PagedResult<TDto>> GetPagedWithTenantAsync(
       DataTableRequest request,
       string? tenantUuid,
       Expression<Func<TEntity, bool>> extraFilter,
       Func<TEntity, TDto> mapFunc)
        {
            var tenantId = await ResolveTenantIdAsync(tenantUuid);
            var search = request.Search?.Trim().ToLower();
            var tenantIdsMatched = await GetTenantIdsBySearchAsync(search);

            // Tenant filter (if a specific tenant selected)
            Expression<Func<TEntity, bool>> tenantFilter = x =>
                !tenantId.HasValue || EF.Property<int>(x, "TenantId") == tenantId.Value;

            // Search filter (platform/profile etc.)
            Expression<Func<TEntity, bool>>? searchFilter = null;
            if (!string.IsNullOrWhiteSpace(search))
            {
                searchFilter = BuildSearchFilter(search) as Expression<Func<TEntity, bool>>;
            }

            // If tenant names matched the search, build a tenant-id filter that EF can translate.
            Expression<Func<TEntity, bool>>? tenantNameMatchFilter = null;
            if (tenantIdsMatched != null && tenantIdsMatched.Count > 0)
            {
                var ids = tenantIdsMatched; // capture
                tenantNameMatchFilter = x => ids.Contains(EF.Property<int>(x, "TenantId"));
            }

            // Combine searchFilter OR tenantNameMatchFilter into a single search expression (if both exist)
            Expression<Func<TEntity, bool>>? combinedSearchOrTenantFilter = null;
            if (searchFilter != null && tenantNameMatchFilter != null)
            {
                combinedSearchOrTenantFilter = CombineFiltersOr(searchFilter, tenantNameMatchFilter);
            }
            else if (searchFilter != null)
            {
                combinedSearchOrTenantFilter = searchFilter;
            }
            else if (tenantNameMatchFilter != null)
            {
                combinedSearchOrTenantFilter = tenantNameMatchFilter;
            }

            // Start with tenantFilter and AND with combinedSearchOrTenantFilter and extraFilter where applicable
            Expression<Func<TEntity, bool>> finalFilter = tenantFilter;

            if (combinedSearchOrTenantFilter != null)
            {
                finalFilter = CombineFilters(finalFilter, combinedSearchOrTenantFilter);
            }

            if (extraFilter != null)
            {
                finalFilter = CombineFilters(finalFilter, extraFilter);
            }

            // Sorting
            var orderBy = BuildSortExpression(request.SortColumn, request.SortDirection);

            // Fetch paged data
            var result = await _repository.GetPagedAsync(
                finalFilter,
                request,
                orderBy,
                q => q.IgnoreQueryFilters());

            // Map to DTOs
            var dtoList = _mapper.Map<List<TDto>>(result.Items);

            // Populate TenantName on DTOs server-side (safe, not part of EF expression)
            try
            {
                var entities = result.Items;
                var tenantIds = entities.Select(e => e.TenantId).Distinct().ToList();

                if (tenantIds.Count > 0)
                {
                    var tenants = await _tenantRepository.GetAllActiveAsync();
                    var tenantDict = tenants
                        .Where(t => tenantIds.Contains((int)t.Id))
                        .ToDictionary(t => (int)t.Id, t => t.TenantName);

                    var tenantNameProp = typeof(TDto).GetProperty("TenantName");
                    if (tenantNameProp != null && tenantNameProp.CanWrite)
                    {
                        for (int i = 0; i < entities.Count; i++)
                        {
                            var tid = entities[i].TenantId;
                            tenantDict.TryGetValue(tid, out var tenantName);
                            tenantNameProp.SetValue(dtoList[i], tenantName);
                        }
                    }
                }
            }
            catch
            {
                // swallow — populate TenantName is an enhancement for admin UI; failing it should not break listing.
            }

            return new PagedResult<TDto>
            {
                Items = dtoList,
                TotalCount = result.TotalCount,
                PageNumber = result.PageNumber,
                PageSize = result.PageSize
            };
        }
        private Expression<Func<TEntity, bool>> CombineFilters(
    Expression<Func<TEntity, bool>> first,
    Expression<Func<TEntity, bool>> second)
        {
            var param = Expression.Parameter(typeof(TEntity));

            var body = Expression.AndAlso(
                Expression.Invoke(first, param),
                Expression.Invoke(second, param));

            return Expression.Lambda<Func<TEntity, bool>>(body, param);
        }

        private Expression<Func<TEntity, bool>> CombineFiltersOr(
    Expression<Func<TEntity, bool>> first,
    Expression<Func<TEntity, bool>> second)
        {
            var param = Expression.Parameter(typeof(TEntity));

            var body = Expression.OrElse(
                Expression.Invoke(first, param),
                Expression.Invoke(second, param));

            return Expression.Lambda<Func<TEntity, bool>>(body, param);
        }
        //    public virtual async Task<PagedResult<TDto>> GetPagedWithTenantAsync(
        //   DataTableRequest request,
        //   string? tenantUuid,
        //   Expression<Func<TEntity, bool>> extraFilter,
        //   Func<TEntity, TDto> mapFunc)
        //    {
        //        var tenantId = await ResolveTenantIdAsync(tenantUuid);
        //        var search = request.Search?.Trim().ToLower();
        //        var tenantIdsMatched = await GetTenantIdsBySearchAsync(search);

        //        // Tenant filter
        //        Expression<Func<TEntity, bool>> tenantFilter = x =>
        //            !tenantId.HasValue || EF.Property<int>(x, "TenantId") == tenantId.Value;

        //        // Search filter
        //        Expression<Func<TEntity, bool>>? searchFilter = null;
        //        if (!string.IsNullOrWhiteSpace(search))
        //        {
        //            searchFilter = BuildSearchFilter(search) as Expression<Func<TEntity, bool>>;
        //        }

        //        // Start combining filters
        //        Expression<Func<TEntity, bool>> finalFilter = tenantFilter;

        //        if (searchFilter != null)
        //        {
        //            finalFilter = CombineFilters(finalFilter, searchFilter);
        //        }

        //        if (extraFilter != null)
        //        {
        //            finalFilter = CombineFilters(finalFilter, extraFilter);
        //        }

        //        // Sorting
        //        var orderBy = BuildSortExpression(request.SortColumn, request.SortDirection);

        //        // Fetch paged data
        //        var result = await _repository.GetPagedAsync(
        //            finalFilter,
        //            request,
        //            orderBy,
        //            q => q.IgnoreQueryFilters());

        //        return new PagedResult<TDto>
        //        {
        //            Items = _mapper.Map<List<TDto>>(result.Items),
        //            TotalCount = result.TotalCount,
        //            PageNumber = result.PageNumber,
        //            PageSize = result.PageSize
        //        };
        //    }
        //    private Expression<Func<TEntity, bool>> CombineFilters(
        //Expression<Func<TEntity, bool>> first,
        //Expression<Func<TEntity, bool>> second)
        //    {
        //        var param = Expression.Parameter(typeof(TEntity));

        //        var body = Expression.AndAlso(
        //            Expression.Invoke(first, param),
        //            Expression.Invoke(second, param));

        //        return Expression.Lambda<Func<TEntity, bool>>(body, param);
        //    }
        #endregion

        #region Common Overrides
        // Add this method in WLBaseService
        /*protected async Task<TEntity?> GetByUuidIgnoringTenantFilterAsync(string uuid)
        {
            var result = await _repository.GetPagedAsync(
                x => x.UUID == uuid,
                new PaginationParams { PageNumber = 1, PageSize = 1 },
                null,
                q => q.IgnoreQueryFilters());

            return result.Items.FirstOrDefault();
        }

        public override async Task<bool> ToggleActiveAsync(string uuid, string userUuid, string ip)
        {
            var entity = await GetByUuidIgnoringTenantFilterAsync(uuid)
                ?? throw new Exception("Record not found");

            entity.IsActive = !entity.IsActive;
            await _repository.UpdateAsync(entity);

            return entity.IsActive;
        }

        public override async Task DeleteAsync(string uuid, string userUuid, string ip)
        {
            var entity = await GetByUuidIgnoringTenantFilterAsync(uuid)
                ?? throw new Exception("Record not found");

            entity.IsActive = false;
            await _repository.UpdateAsync(entity);
        }

        protected override async Task UpdateAsync(TCommand command, string userUuid, string ip)
        {
            var entity = await GetByUuidIgnoringTenantFilterAsync(command.UUID!)
                ?? throw new Exception("Record not found");

            _mapper.Map(command, entity);
            await _repository.UpdateAsync(entity);
        }

        public override async Task<TDto?> GetByUuidAsync(string uuid)
        {
            if (string.IsNullOrWhiteSpace(uuid))
                return null;

            var entity = await GetByUuidIgnoringTenantFilterAsync(uuid);
            return entity == null ? null : _mapper.Map<TDto>(entity);
        }*/

        #endregion
    }
}
