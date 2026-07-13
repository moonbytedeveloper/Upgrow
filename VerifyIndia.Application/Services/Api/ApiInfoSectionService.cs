using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Commands.Api;
using Upgrow.Application.DTO.Api;
using Upgrow.Application.IServices.Api;
using Upgrow.Application.Services.Master;
using Upgrow.Domain.Entities;
using Upgrow.Domain.IRepositories;

namespace Upgrow.Application.Services.Api
{
    public class ApiInfoSectionService : MasterServiceBase<ApiInfoSection, ApiInfoSectionDto, ApiInfoSectionCommand>, IApiInfoSectionService

    {
        public ApiInfoSectionService(IMasterRepository<ApiInfoSection> repository, IMapper mapper)
           : base(repository, mapper) { }

        public async Task<List<ApiInfoSectionDto>> GetAllActiveAsync()
        {
            var entities = await _repository.GetAllActiveAsync();
            return _mapper.Map<List<ApiInfoSectionDto>>(entities);
        }

        protected override async Task<bool> IsDuplicateAsync(ApiInfoSectionCommand command)
        {
            return await _repository.ExistsAsync(x =>
                 (x.Title!.ToLower().Trim() == command.Title.ToLower().Trim() &&
                 x.ApiUUID!.ToLower().Trim() == command.ApiUUID.ToLower().Trim()) ||
                 (x.Sequence  == command.Sequence &&
                 x.ApiUUID!.ToLower().Trim() == command.ApiUUID.ToLower().Trim()
                 ) &&
                x.UUID != command.UUID );
        }

        protected override Expression<Func<ApiInfoSection, bool>>? BuildSearchFilter(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();

            return x =>
                 (x.Title != null && x.Title.ToLower().Contains(searchTerm)) ||
                 (x.ApiUUID != null && x.ApiUUID.ToLower().Contains(searchTerm)) ||
                 (x.Description != null && x.Description.ToLower().Contains(searchTerm));
        }


        protected override Func<IQueryable<ApiInfoSection>, IOrderedQueryable<ApiInfoSection>>? BuildSortExpression(
            string? sortColumn,
            string? sortDirection)
        {
            var isAsc = sortDirection?.ToLower() == "asc";

            return sortColumn?.ToLower() switch
            {
                "title" => q => isAsc ? q.OrderBy(x => x.Title) : q.OrderByDescending(x => x.Title),
                "api" => q => isAsc ? q.OrderBy(x => x.ApiUUID) : q.OrderByDescending(x => x.ApiUUID),
                "sequence" => q => isAsc ? q.OrderBy(x => x.Sequence) : q.OrderByDescending(x => x.Sequence),
                "description" => q => isAsc ? q.OrderBy(x => x.Description) : q.OrderByDescending(x => x.Description),
                _ => q => isAsc ? q.OrderBy(x => x.Id) : q.OrderByDescending(x => x.Id)
            };
        }
 
    }
}

