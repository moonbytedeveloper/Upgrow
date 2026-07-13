using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VerifyIndia.Application.Commands.Api;
using VerifyIndia.Application.DTO.Api;
using VerifyIndia.Application.IServices.Api;
using VerifyIndia.Application.Services.Master;
using VerifyIndia.Domain.Entities;
using VerifyIndia.Domain.IRepositories;

namespace VerifyIndia.Application.Services.Api
{
    public class ApiProviderComponentMappingService : MasterServiceBase<Api_ProviderComponentMapping, ApiProviderComponentMappingDto, ApiProviderComponentMappingCommand>, IApiProviderComponentMappingService
    {
        public ApiProviderComponentMappingService(IMasterRepository<Api_ProviderComponentMapping> repository, IMapper mapper)
           : base(repository, mapper) { }

        // Define which field to check for duplicates
        protected override async Task<bool> IsDuplicateAsync(ApiProviderComponentMappingCommand command)
        {
            return await _repository.ExistsAsync(x =>
                x.ApiUUID!.ToLower().Trim() == command.ApiUUID.ToLower().Trim() &&
                x.ComponentUUID!.ToLower().Trim() == command.ComponentUUID.ToLower().Trim() &&
                x.Sequence == command.Sequence &&
                x.UUID != command.UUID);
        }

        // Define which fields to search
        protected override Expression<Func<Api_ProviderComponentMapping, bool>>? BuildSearchFilter(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();

            return x =>
                (x.ApiUUID != null && x.ApiUUID.ToLower().Contains(searchTerm)) ||
                (x.ComponentUUID != null && x.ComponentUUID.ToLower().Contains(searchTerm));
        }

        // Define sorting (optional - remove if default Id sorting is fine)
        protected override Func<IQueryable<Api_ProviderComponentMapping>, IOrderedQueryable<Api_ProviderComponentMapping>>? BuildSortExpression(
            string? sortColumn,
            string? sortDirection)
        {
            var isAsc = sortDirection?.ToLower() == "asc";

            return sortColumn?.ToLower() switch
            {
                "api" => q => isAsc ? q.OrderBy(x => x.ApiUUID) : q.OrderByDescending(x => x.ApiUUID),
                "component" => q => isAsc ? q.OrderBy(x => x.ComponentUUID) : q.OrderByDescending(x => x.ComponentUUID),
                _ => q => isAsc ? q.OrderBy(x => x.Id) : q.OrderByDescending(x => x.Id)
            };
        }

        public async Task<List<ApiProviderComponentMappingDto>> GetByApiUuidAsync(string apiUuid)
        {
            var entities = await _repository.FindAllAsync(x => x.ApiUUID == apiUuid);

            return _mapper.Map<List<ApiProviderComponentMappingDto>>(entities);
        }

    }
}
