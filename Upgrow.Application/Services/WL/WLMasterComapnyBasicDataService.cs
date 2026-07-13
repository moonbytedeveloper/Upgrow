using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Commands.Master;
using Upgrow.Application.Commands.WL;
using Upgrow.Application.DTO.DropDown;
using Upgrow.Application.DTO.Master;
using Upgrow.Application.DTO.WL;
using Upgrow.Application.DTOs;
using Upgrow.Application.IServices.Master;
using Upgrow.Application.IServices.WL;
using Upgrow.Application.Services.Master;
using Upgrow.Domain.Common;
using Upgrow.Domain.Entities;
using Upgrow.Domain.IRepositories;

namespace Upgrow.Application.Services.WL
{
    public class WLMasterComapnyBasicDataService : WLBaseService<WL_MasterCompanyBasicData, WLMasterCompanyBasicDataDto, WLMasterCompanyBasicDataCommand>, IWLMasterCompanyBasicDataService
    {

        public WLMasterComapnyBasicDataService(
            IMasterRepository<WL_MasterCompanyBasicData> repository,
            IMasterRepository<Tenant> tenantRepository,
            IMapper mapper)
            : base(repository, tenantRepository, mapper)
        {

        }
        protected override string EntityDisplayName => "Company Basic Data";
        public async Task<WLMasterCompanyBasicDataDto?> GetFirstAsync()
        {
            var entity = (await _repository.GetAllActiveAsync()).FirstOrDefault();
            return entity == null ? null : _mapper.Map<WLMasterCompanyBasicDataDto>(entity);
        }
        protected override Expression<Func<WL_MasterCompanyBasicData, bool>>? BuildSearchFilter(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();
            return x =>
                (x.CompName != null && x.CompName.ToLower().Contains(searchTerm)) ||
                (x.TenantName != null && x.TenantName.ToLower().Contains(searchTerm)) ||
                (x.EmailId != null && x.EmailId.ToLower().Contains(searchTerm)) ||
                (x.Address != null && x.Address.ToLower().Contains(searchTerm));
        }

        protected override async Task<bool> IsDuplicateAsync(WLMasterCompanyBasicDataCommand command)
        {
            return await _repository.ExistsAsync(x =>
                x.CompName!.ToLower().Trim() == command.CompName.ToLower().Trim() &&
                x.TenantId == command.TenantId &&
                x.UUID != command.UUID);
        }

        protected override Func<IQueryable<WL_MasterCompanyBasicData>, IOrderedQueryable<WL_MasterCompanyBasicData>>? BuildSortExpression(
            string? sortColumn,
            string? sortDirection)
        {
            var isAsc = sortDirection?.ToLower() == "asc";

            return sortColumn?.ToLower() switch
            {
                "compname" => q => isAsc ? q.OrderBy(x => x.CompName) : q.OrderByDescending(x => x.CompName),
                "emailid" => q => isAsc ? q.OrderBy(x => x.EmailId) : q.OrderByDescending(x => x.EmailId),
                "phone" => q => isAsc ? q.OrderBy(x => x.Phone) : q.OrderByDescending(x => x.Phone),
                "address" => q => isAsc ? q.OrderBy(x => x.Address) : q.OrderByDescending(x => x.Address),
                _ => q => isAsc ? q.OrderBy(x => x.Id) : q.OrderByDescending(x => x.Id)
            };
        }
    }
}
