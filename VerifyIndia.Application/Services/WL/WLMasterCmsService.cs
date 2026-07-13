using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Commands.WL;
using Upgrow.Application.DTO.DropDown;
using Upgrow.Application.DTO.WL;
using Upgrow.Application.DTOs.Master;
using Upgrow.Application.IServices.WL;
using Upgrow.Domain.Entities;
using Upgrow.Domain.IRepositories;

namespace Upgrow.Application.Services.WL
{
    public class WLMasterCmsService : WLBaseService<WL_MasterCMS, WLMasterCMSDto, WLMasterCMSCommand>, IWLMasterCmsService
    {
        public WLMasterCmsService(
            IMasterRepository<WL_MasterCMS> repository,
            IMasterRepository<Tenant> tenantRepository,
            IMapper mapper)
            : base(repository, tenantRepository, mapper)
        {
           
        }
        protected override string EntityDisplayName => "CMS";
        protected override Expression<Func<WL_MasterCMS, bool>>? BuildSearchFilter(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();
            return x =>
                (x.TenantName != null && x.TenantName.ToLower().Contains(searchTerm)) ||
                (x.PageTitle != null && x.PageTitle.ToLower().Contains(searchTerm));
        }

        protected override async Task<bool> IsDuplicateAsync(WLMasterCMSCommand command)
        {
            return await _repository.ExistsAsync(x =>
                x.TenantId! == command.TenantId &&
                x.PageTitle!.ToLower().Trim() == command.PageTitle.ToLower().Trim() &&
                x.UUID != command.UUID);
        }

        protected override Func<IQueryable<WL_MasterCMS>, IOrderedQueryable<WL_MasterCMS>>? BuildSortExpression(
            string? sortColumn,
            string? sortDirection)
        {
            var isAsc = sortDirection?.ToLower() == "asc";

            return sortColumn?.ToLower() switch
            {
                "tenant" => q => isAsc ? q.OrderBy(x => x.TenantName) : q.OrderByDescending(x => x.TenantName),
                "pagename" => q => isAsc ? q.OrderBy(x => x.PageTitle) : q.OrderByDescending(x => x.PageTitle),

                _ => q => isAsc ? q.OrderBy(x => x.Id) : q.OrderByDescending(x => x.Id)
            };
        }
    }
}
