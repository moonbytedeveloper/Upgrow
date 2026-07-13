using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Commands.Master;
using VerifyIndia.Application.Commands.Website;
using VerifyIndia.Application.Commands.WL;
using VerifyIndia.Application.DTO.Master;
using VerifyIndia.Application.DTO.WL;
using VerifyIndia.Application.DTOs;
using VerifyIndia.Application.IServices.WL;
using VerifyIndia.Application.Services.Master;
using VerifyIndia.Domain.Common;
using VerifyIndia.Domain.Entities;
using VerifyIndia.Domain.IRepositories;

namespace VerifyIndia.Application.Services.WL
{
    public class WLTenantDomainService :  MasterServiceBase<TenantDomain, WLTenantDomainDto, WLTenantDomainCommand>, IWLTenantDomainService
    {
        private readonly IMasterRepository<Tenant> _tenantRepository;

        public WLTenantDomainService(IMasterRepository<TenantDomain> repository, IMapper mapper,
            IMasterRepository<Tenant> tenantRepository )
            : base(repository, mapper)
        {
            _tenantRepository = tenantRepository;
        }

        // Define which field to check for duplicates
        protected override async Task<bool> IsDuplicateAsync(WLTenantDomainCommand command)
        {
            return await _repository.ExistsAsync(x =>
                x.Domain!.ToLower().Trim() == command.Domain.ToLower().Trim() &&
                x.UUID != command.UUID);
        }

        // Define which fields to search
        protected override Expression<Func<TenantDomain, bool>>? BuildSearchFilter(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();

            return x =>
                (x.Domain != null && x.Domain.ToLower().Contains(searchTerm)) ||
                (x.DomainType != null && x.DomainType.ToLower().Contains(searchTerm));
        }

        // Define sorting
        protected override Func<IQueryable<TenantDomain>, IOrderedQueryable<TenantDomain>>? BuildSortExpression(
            string? sortColumn,
            string? sortDirection)
        {
            var isAsc = sortDirection?.ToLower() == "asc";

            return sortColumn?.ToLower() switch
            {
                "title" => q => isAsc ? q.OrderBy(x => x.Domain) : q.OrderByDescending(x => x.Domain),

                _ => q => isAsc ? q.OrderBy(x => x.Id) : q.OrderByDescending(x => x.Id)
            };
        }

       

    }
}



