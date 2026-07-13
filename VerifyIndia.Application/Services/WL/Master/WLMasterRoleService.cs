using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Commands.Master;
using VerifyIndia.Application.Commands.WL.Master;
using VerifyIndia.Application.DTO.Master;
using VerifyIndia.Application.DTO.WL.Master;
using VerifyIndia.Application.IServices.Master;
using VerifyIndia.Application.Services.Master;
using VerifyIndia.Domain.Entities;
using VerifyIndia.Domain.Entities.WL.Master;
using VerifyIndia.Domain.IRepositories;

namespace VerifyIndia.Application.Services.WL.Master
{
    public class WLMasterRoleService : MasterServiceBase<WL_MasterRoles, WLMasterRolesDto, WLMasterRolesCommand>, IWLMasterRoleService
    {
        public WLMasterRoleService(IMasterRepository<WL_MasterRoles> repository, IMapper mapper)
            : base(repository, mapper) { }

        public async Task<List<WLMasterRolesDto>> GetAllActiveAsync()
        {
            var entities = await _repository.GetAllActiveAsync();
            return _mapper.Map<List<WLMasterRolesDto>>(entities);
        }
        // Define which field to check for duplicates
        protected override async Task<bool> IsDuplicateAsync(WLMasterRolesCommand command)
        {
            return await _repository.ExistsAsync(x =>
                x.Title!.ToLower().Trim() == command.Title.ToLower().Trim() &&
                x.UUID != command.UUID);
        }

        // Define which fields to search
        protected override Expression<Func<WL_MasterRoles, bool>>? BuildSearchFilter(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();

            return x =>
                (x.Title != null && x.Title.ToLower().Contains(searchTerm)) ||
                (x.ShortTitle != null && x.ShortTitle.ToLower().Contains(searchTerm));
        }

        // Define sorting (optional - remove if default Id sorting is fine)
        protected override Func<IQueryable<WL_MasterRoles>, IOrderedQueryable<WL_MasterRoles>>? BuildSortExpression(
            string? sortColumn,
            string? sortDirection)
        {
            var isAsc = sortDirection?.ToLower() == "asc";

            return sortColumn?.ToLower() switch
            {
                "name" => q => isAsc ? q.OrderBy(x => x.Title) : q.OrderByDescending(x => x.Title),
                "shortname" => q => isAsc ? q.OrderBy(x => x.ShortTitle) : q.OrderByDescending(x => x.ShortTitle),
                _ => q => isAsc ? q.OrderBy(x => x.Id) : q.OrderByDescending(x => x.Id)
            };
        }
    }
}
