using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Commands.Api;
using VerifyIndia.Application.DTO.Api;
using VerifyIndia.Application.IServices.Api;
using VerifyIndia.Application.Services.Master;
using VerifyIndia.Domain.Entities;
using VerifyIndia.Domain.IRepositories;

namespace VerifyIndia.Application.Services.Api
{
    public class ApiEndpointService : MasterServiceBase<Api_Endpoint, ApiEndpointDto, ApiEndpointCommand>, IApiEndpointService

    {
        public ApiEndpointService(IMasterRepository<Api_Endpoint> repository, IMapper mapper)
           : base(repository, mapper) { }

        public async Task<List<ApiEndpointDto>> GetAllActiveAsync()
        {
            var entities = await _repository.GetAllActiveAsync();
            return _mapper.Map<List<ApiEndpointDto>>(entities);
        }

        public async Task<List<ApiEndpointDto>> GetByApiAsync(string apiUuid)
        {
            var entities = await _repository.FindAllAsync(x => x.ApiUUID == apiUuid);
            return _mapper.Map<List<ApiEndpointDto>>(entities);
        }

        public async Task<ApiEndpointDto> GetByApiUuidAsync(string apiUuid)
        {
            var entities = await _repository.FindAllAsync(x => x.ApiUUID == apiUuid);
            var entity = entities?.FirstOrDefault();
            return _mapper.Map<ApiEndpointDto>(entity);
        }

        // Define which field to check for duplicates
        protected override async Task<bool> IsDuplicateAsync(ApiEndpointCommand command)
        {
            return await _repository.ExistsAsync(x =>
                 x.ApiUUID!.ToLower().Trim() == command.ApiUUID.ToLower().Trim() &&
                 x.EndpointUrl!.ToLower().Trim() == command.EndpointUrl.ToLower().Trim() &&
                 x.HttpMethod!.ToLower().Trim() == command.HttpMethod.ToLower().Trim() &&
                x.UUID != command.UUID);
        }

        // Define which fields to search
        protected override Expression<Func<Api_Endpoint, bool>>? BuildSearchFilter(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();

            return x =>
                 (x.HttpMethod != null && x.HttpMethod.ToLower().Contains(searchTerm)) ||
                  (x.EndpointUrl != null && x.EndpointUrl.ToLower().Contains(searchTerm)) ||
                (x.ApiUUID != null && x.ApiUUID.ToLower().Contains(searchTerm));
        }

        // Define sorting (optional - remove if default Id sorting is fine)
        protected override Func<IQueryable<Api_Endpoint>, IOrderedQueryable<Api_Endpoint>>? BuildSortExpression(
            string? sortColumn,
            string? sortDirection)
        {
            var isAsc = sortDirection?.ToLower() == "asc";

            return sortColumn?.ToLower() switch
            {
                "api" => q => isAsc ? q.OrderBy(x => x.ApiUUID) : q.OrderByDescending(x => x.ApiUUID),
                "endpoint" => q => isAsc ? q.OrderBy(x => x.EndpointUrl) : q.OrderByDescending(x => x.EndpointUrl),
                "httpmethod" => q => isAsc ? q.OrderBy(x => x.HttpMethod) : q.OrderByDescending(x => x.HttpMethod),
                _ => q => isAsc ? q.OrderBy(x => x.Id) : q.OrderByDescending(x => x.Id)
            };
        }





    }
}
   
   