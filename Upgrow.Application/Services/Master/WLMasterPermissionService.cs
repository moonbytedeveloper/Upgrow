using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Commands.Master;
using Upgrow.Application.DTO.Master;
using Upgrow.Application.IServices.Master;
using Upgrow.Domain.Entities;
using Upgrow.Domain.IRepositories;

namespace Upgrow.Application.Services.Master
{
    public class WLMasterPermissionService : MasterServiceBase<WL_MasterPermission, WLMasterPermissionDto, WLMasterPermissionCommand>, IWLMasterPermissionService
    {
        public WLMasterPermissionService(IMasterRepository<WL_MasterPermission> repository, IMapper mapper)
            : base(repository, mapper) { }
        public async Task<List<WLMasterPermissionDto>> GetAllActiveAsync()
        {
            var entities = await _repository.GetAllActiveAsync();
            return _mapper.Map<List<WLMasterPermissionDto>>(entities);
        }

        // Define which field to check for duplicates
        protected override async Task<bool> IsDuplicateAsync(WLMasterPermissionCommand command)
        {
            return await _repository.ExistsAsync(x =>
                x.Name!.ToLower().Trim() == command.Name.ToLower().Trim() &&
                x.UUID != command.UUID);
        }

        // Define which fields to search
        protected override Expression<Func<WL_MasterPermission, bool>>? BuildSearchFilter(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();

            return x =>
                (x.Name != null && x.Name.ToLower().Contains(searchTerm)) ||
                (x.Description != null && x.Description.ToLower().Contains(searchTerm));
        }

        protected override Func<IQueryable<WL_MasterPermission>, IOrderedQueryable<WL_MasterPermission>>? BuildSortExpression(
            string? sortColumn,
            string? sortDirection)
        {
            var isAsc = sortDirection?.ToLower() == "asc";

            return sortColumn?.ToLower() switch
            {
                "name" => q => isAsc ? q.OrderBy(x => x.Name) : q.OrderByDescending(x => x.Name),
                "description" => q => isAsc ? q.OrderBy(x => x.Description) : q.OrderByDescending(x => x.Description),
                "permissionGroupTitle" => q => isAsc ? q.OrderBy(x => x.PermissionGroupUUID) : q.OrderByDescending(x => x.PermissionGroupUUID),
                _ => q => isAsc ? q.OrderBy(x => x.Id) : q.OrderByDescending(x => x.Id)
            };
        }
    }
}