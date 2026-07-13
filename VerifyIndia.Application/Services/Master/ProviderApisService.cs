using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Commands.Master;
using VerifyIndia.Application.DTO.Api;
using VerifyIndia.Application.DTO.Master;
using VerifyIndia.Application.IServices.Master;
using VerifyIndia.Domain.Entities;
using VerifyIndia.Domain.IRepositories;

namespace VerifyIndia.Application.Services.Master
{
    public class ProviderApisService : MasterServiceBase<Provider_Apis, ProviderApisDto, ProviderApisCommand>, IProviderApisService
    {
        public ProviderApisService(IMasterRepository<Provider_Apis> repository, IMapper mapper)
            : base(repository, mapper) { }

        public async Task<List<ProviderApisDto>> GetAllActiveAsync()
        {
            var entities = await _repository.GetAllActiveAsync();
            return _mapper.Map<List<ProviderApisDto>>(entities);
        }
        protected override Expression<Func<Provider_Apis, bool>>? BuildSearchFilter(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();
            return x =>
                (x.ProviderUUID != null && x.ProviderUUID.ToLower().Contains(searchTerm));
        }
        protected override async Task<bool> IsDuplicateAsync(ProviderApisCommand command)
        {
            return await _repository.ExistsAsync(x =>
                x.ApiUUID == command.ApiUUID &&
                x.ProviderUUID == command.ProviderUUID &&
                x.UUID != command.UUID);
        }

        protected override Func<IQueryable<Provider_Apis>, IOrderedQueryable<Provider_Apis>>? BuildSortExpression(
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
        public async Task<List<ProviderApisDto>> GetByApiUuidAsync(string apiUuid)
        {
            var entities = await _repository.FindAllAsync(x => x.ApiUUID == apiUuid);

            return _mapper.Map<List<ProviderApisDto>>(entities);
        }
    }
}