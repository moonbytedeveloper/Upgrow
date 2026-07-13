using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Commands.Master;
using VerifyIndia.Application.Commands.WL;
using VerifyIndia.Application.DTO.Master;
using VerifyIndia.Application.DTO.WL;
using VerifyIndia.Application.IServices.Master;
using VerifyIndia.Application.IServices.WL;
using VerifyIndia.Application.Services.Master;
using VerifyIndia.Domain.Entities;
using VerifyIndia.Domain.IRepositories;

namespace VerifyIndia.Application.Services.WL
{
    public class WLTenantService : MasterServiceBase<Tenant, WLTenantDto, WLTenantCommand>, IWLTenantService
    {
        public WLTenantService(IMasterRepository<Tenant> repository, IMapper mapper)
           : base(repository, mapper) { }

        protected override async Task<bool> IsDuplicateAsync(WLTenantCommand command)
        {
            return await _repository.ExistsAsync(x =>
                x.Identifier!.ToLower().Trim() == command.Identifier.ToLower().Trim() &&
                x.UUID != command.UUID);
        }

        protected override Expression<Func<Tenant, bool>>? BuildSearchFilter(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();

            return x => 
            (x.TenantName != null && x.TenantName.ToLower().Contains(searchTerm)) ||
            (x.Identifier != null && x.Identifier.ToLower().Contains(searchTerm));
        }

        protected override Func<IQueryable<Tenant>, IOrderedQueryable<Tenant>>? BuildSortExpression(
            string? sortColumn,
            string? sortDirection)
        {
            var isAsc = sortDirection?.ToLower() == "asc";

            return sortColumn?.ToLower() switch
            {
                "path" => q => isAsc ? q.OrderBy(x => x.TenantName) : q.OrderByDescending(x => x.TenantName),
                _ => q => q.OrderByDescending(x => x.Id)
            };
        }
        public async Task<List<WLTenantDto>> GetAllAsync()
        {
            var entities = await _repository.GetAllActiveAsync();
            return entities
           .Where(t => !t.IsPlatformOwner)
           .OrderBy(t => t.TenantName)
           .Select(t => new WLTenantDto
           {
            UUID = t.UUID,   
            TenantName = t.TenantName
           })
           .ToList();
        }

        public async Task<List<WLTenantDto>> GetAllActiveAsync()
        {
            var entities = await _repository.GetAllActiveAsync();
            var list = entities
               .Where(t => !t.IsPlatformOwner)
               .OrderBy(t => t.TenantName)
               .Select(t => new { id = t.Id, text = t.TenantName })
               .ToList();

            return _mapper.Map<List<WLTenantDto>>(list);
        }
    }
}

    
