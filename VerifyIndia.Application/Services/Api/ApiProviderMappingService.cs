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
using VerifyIndia.Application.DTO.Master;
using VerifyIndia.Application.IServices.Api;
using VerifyIndia.Application.Services.Master;
using VerifyIndia.Domain.Entities;
using VerifyIndia.Domain.IRepositories;

namespace VerifyIndia.Application.Services.Api
{
    public class ApiProviderMappingService : MasterServiceBase<Api_ProviderMapping, ApiProviderMappingDto, ApiProviderMappingCommand>, IApiProviderMappingService
    {
        public ApiProviderMappingService(IMasterRepository<Api_ProviderMapping> repository, IMapper mapper)
            : base(repository, mapper) { }

        public async Task<List<ApiProviderMappingDto>> GetAllActiveAsync()
        {
            var entities = await _repository.GetAllActiveAsync();
            return _mapper.Map<List<ApiProviderMappingDto>>(entities);
        }

        // Define which field to check for duplicates
        protected override async Task<bool> IsDuplicateAsync(ApiProviderMappingCommand command)
        {
            return await _repository.ExistsAsync(x =>
                x.ApiUUID.ToLower().Trim() == command.ApiUUID.ToLower().Trim() &&
                x.ProviderUUID.ToLower().Trim() == command.ProviderUUID.ToLower().Trim() &&
                x.UUID != command.UUID);
        }

        // Define which fields to search
        protected override Expression<Func<Api_ProviderMapping, bool>>? BuildSearchFilter(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();

            return x =>
                (x.ProviderUUID != null && x.ProviderUUID.ToLower().Contains(searchTerm)) ||
                (x.ApiUUID != null && x.ApiUUID.ToLower().Contains(searchTerm));
        }

        // Define sorting (optional - remove if default Id sorting is fine)
        protected override Func<IQueryable<Api_ProviderMapping>, IOrderedQueryable<Api_ProviderMapping>>? BuildSortExpression(
            string? sortColumn,
            string? sortDirection)
        {
            var isAsc = sortDirection?.ToLower() == "asc";

            return sortColumn?.ToLower() switch
            {
                "provider" => q => isAsc ? q.OrderBy(x => x.ProviderUUID) : q.OrderByDescending(x => x.ProviderUUID),
                "api" => q => isAsc ? q.OrderBy(x => x.ApiUUID) : q.OrderByDescending(x => x.ApiUUID),
                _ => q => isAsc ? q.OrderBy(x => x.Id) : q.OrderByDescending(x => x.Id)
            };
        }

        public async Task<List<ApiProviderMappingDto>> GetByApiUuidAsync(string apiUuid)
        {
            var entities = await _repository.FindAllAsync(x => x.ApiUUID == apiUuid);

            return _mapper.Map<List<ApiProviderMappingDto>>(entities);
        }



    }
}
   
    
