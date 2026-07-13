using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Commands.WL;
using Upgrow.Application.DTO.WL;
using Upgrow.Application.IServices.WL;
using Upgrow.Domain.Entities;
using Upgrow.Domain.IRepositories;

namespace Upgrow.Application.Services.WL
{
    public class WLClientsService : WLBaseService<WL_Clients, WLClientsDto, WLClientsCommand>, IWLClientsService
    {
        public WLClientsService(
           IMasterRepository<WL_Clients> repository,
           IMasterRepository<Tenant> tenantRepository,
           IMapper mapper)
           : base(repository, tenantRepository, mapper)
        {

        }
        protected override Expression<Func<WL_Clients, bool>>? BuildSearchFilter(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();
            return x =>
                (x.Name != null && x.Name.ToLower().Contains(searchTerm) ||
                 (x.TenantName != null && x.TenantName.ToLower().Contains(searchTerm)));
        }

        protected override async Task<bool> IsDuplicateAsync(WLClientsCommand command)
        {
            return await _repository.ExistsAsync(x =>
                x.Name!.ToLower().Trim() == command.Name.ToLower().Trim() &&
                x.UUID != command.UUID);
        }

        protected override Func<IQueryable<WL_Clients>, IOrderedQueryable<WL_Clients>>? BuildSortExpression(
            string? sortColumn,
            string? sortDirection)
        {
            var isAsc = sortDirection?.ToLower() == "asc";

            return sortColumn?.ToLower() switch
            {
                "name" => q => isAsc ? q.OrderBy(x => x.Name) : q.OrderByDescending(x => x.Name),
                _ => q => isAsc ? q.OrderBy(x => x.Id) : q.OrderByDescending(x => x.Id)
            };
        }
    }
}

   
