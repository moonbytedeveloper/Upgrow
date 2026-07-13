using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Commands.Website;
using Upgrow.Application.DTO.Api;
using Upgrow.Application.DTO.CustomerPanel;
using Upgrow.Application.DTO.Website;
using Upgrow.Application.DTOs;
using Upgrow.Application.IServices.Website;
using Upgrow.Application.Services.Master;
using Upgrow.Domain.Common;
using Upgrow.Domain.Entities;
using Upgrow.Domain.IRepositories;

namespace Upgrow.Application.Services.Website
{
    public class KnowledgeHubService : MasterServiceBase<Knowledge_Hub, KnowledgeHubDto, KnowledgeHubCommand>, IKnowledgeHubService
    {
        public KnowledgeHubService(IMasterRepository<Knowledge_Hub> repository, IMapper mapper)
          : base(repository, mapper) { }

        protected override async Task<bool> IsDuplicateAsync(KnowledgeHubCommand command)
        {
            return await _repository.ExistsAsync(x =>
                x.Name!.ToLower().Trim() == command.Name.ToLower().Trim() &&
                x.Description!.ToLower().Trim() == command.Description.ToLower().Trim() &&
                x.UUID != command.UUID);
        }
        // Define which fields to search
        protected override Expression<Func<Knowledge_Hub, bool>>? BuildSearchFilter(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();

            return x =>
                (x.Name != null && x.Name.ToLower().Contains(searchTerm)) ||
                (x.CategoryUUID != null && x.CategoryUUID.ToLower().Contains(searchTerm)) ||
                 (x.Description != null && x.Description.ToLower().Contains(searchTerm));

        }
        // Define sorting (optional - remove if default Id sorting is fine)
        protected override Func<IQueryable<Knowledge_Hub>, IOrderedQueryable<Knowledge_Hub>>? BuildSortExpression(
            string? sortColumn,
            string? sortDirection)
        {
            var isAsc = sortDirection?.ToLower() == "asc";

            return sortColumn?.ToLower() switch
            {
                "name" => q => isAsc ? q.OrderBy(x => x.Name) : q.OrderByDescending(x => x.Name),
                "category" => q => isAsc ? q.OrderBy(x => x.CategoryUUID) : q.OrderByDescending(x => x.CategoryUUID),
                "description" => q => isAsc ? q.OrderBy(x => x.Description) : q.OrderByDescending(x => x.CategoryUUID),
                _ => q => isAsc ? q.OrderBy(x => x.Id) : q.OrderByDescending(x => x.Id)
            };


        }
        public async Task<List<KnowledgeHubDto>> GetAllAsync()
        {
            var entities = await _repository.GetAllActiveAsync();
            return _mapper.Map<List<KnowledgeHubDto>>(entities);
        }

        public async Task<List<KnowledgeHubApiDto>> GetByCategoryAsync(string categoryUuid)
        {
            if (string.IsNullOrWhiteSpace(categoryUuid))
                return new List<KnowledgeHubApiDto>();

            // Use repository FindAllAsync (supports expression) so DB only returns category rows
            var entities = await _repository.FindAllAsync(x => x.IsActive == true && x.CategoryUUID == categoryUuid);
            var ordered = entities.OrderBy(e => e.Id).ToList();
            return _mapper.Map<List<KnowledgeHubApiDto>>(ordered);
        }

        public async Task<PagedResult<KnowledgeHubApiDto>> GetByCategoryPagedAsync(
    string categoryUuid,
    DataTableRequest request)
        {
            if (string.IsNullOrWhiteSpace(categoryUuid))
            {
                return new PagedResult<KnowledgeHubApiDto>
                {
                    Items = [],
                    TotalCount = 0,
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize
                };
            }

            var term = request.Search?.Trim().ToLowerInvariant();
            Expression<Func<Knowledge_Hub, bool>>? searchFilter = string.IsNullOrWhiteSpace(term)
                ? null
                : BuildSearchFilter(term);

            Func<IQueryable<Knowledge_Hub>, IQueryable<Knowledge_Hub>> queryModifier = q =>
            {
                q = q.Where(x => x.IsActive && x.CategoryUUID == categoryUuid);
                if (searchFilter != null)
                {
                    q = q.Where(searchFilter);
                }

                return q;
            };

            var orderBy = BuildSortExpression(request.SortColumn, request.SortDirection);

            var result = await _repository.GetPagedAsync(
                filter: null,
                request,
                orderBy,
                queryModifier);

            return new PagedResult<KnowledgeHubApiDto>
            {
                Items = _mapper.Map<List<KnowledgeHubApiDto>>(result.Items),
                TotalCount = result.TotalCount,
                PageNumber = result.PageNumber,
                PageSize = result.PageSize
            };
        }
    }
}
