using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Commands.Menu;
using VerifyIndia.Application.Commands.WL.Master;
using VerifyIndia.Application.DTO.Menu;
using VerifyIndia.Application.DTO.WL.Master;
using VerifyIndia.Application.IServices.Menu;
using VerifyIndia.Application.IServices.WL.Master;
using VerifyIndia.Application.Services.Master;
using VerifyIndia.Domain.Common;
using VerifyIndia.Domain.Entities;
using VerifyIndia.Domain.Entities.WL.Master;
using VerifyIndia.Domain.IRepositories;

namespace VerifyIndia.Application.Services.WL.Master
{
    public class WLMenuRolePermissionService : MasterServiceBase<WL_MenuRolePermission, WLMenuRolePermissionDto, WLMenuRolePermissionCommand>, IWLMenuRolePermissionService
    {
        public WLMenuRolePermissionService(IMasterRepository<WL_MenuRolePermission> repository, IMapper mapper)
            : base(repository, mapper) { }

        public async Task<List<WLMenuRolePermissionDto>> GetAllActiveAsync()
        {
            var entities = await _repository.GetAllActiveAsync();
            return _mapper.Map<List<WLMenuRolePermissionDto>>(entities);
        }

        public async Task<List<WLMenuRolePermissionDto>> GetAllByRoleAsync(string roleUUID)
        {
            if (string.IsNullOrWhiteSpace(roleUUID))
                return new List<WLMenuRolePermissionDto>();

            Expression<Func<WL_MenuRolePermission, bool>> filter = x => x.RoleUUID == roleUUID && x.IsActive;
            var pagination = new PaginationParams { PageNumber = 1, PageSize = int.MaxValue };

            var result = await _repository.GetPagedAsync(filter, pagination);
            return _mapper.Map<List<WLMenuRolePermissionDto>>(result.Items);
        }


        /// <summary>
        /// Get ALL permissions for a role (active AND inactive)
        /// </summary>
        public async Task<List<WLMenuRolePermissionDto>> GetAllByRoleIncludeInactiveAsync(string roleUUID)
        {
            if (string.IsNullOrWhiteSpace(roleUUID))
                return new List<WLMenuRolePermissionDto>();

            //  get ALL records
            Expression<Func<WL_MenuRolePermission, bool>> filter = x => x.RoleUUID == roleUUID;
            var pagination = new PaginationParams { PageNumber = 1, PageSize = int.MaxValue };

            var result = await _repository.GetPagedAsync(filter, pagination);
            return _mapper.Map<List<WLMenuRolePermissionDto>>(result.Items);
        }

        public async Task<WLMenuRolePermissionDto?> GetByRoleAndPermissionAsync(string roleUUID, string permissionUUID)
        {
            if (string.IsNullOrWhiteSpace(roleUUID) || string.IsNullOrWhiteSpace(permissionUUID))
                return null;

            Expression<Func<WL_MenuRolePermission, bool>> filter = x =>
                x.RoleUUID == roleUUID && x.PermissionUUID == permissionUUID;

            var pagination = new PaginationParams { PageNumber = 1, PageSize = 1 };

            var result = await _repository.GetPagedAsync(filter, pagination);
            var entity = result.Items.FirstOrDefault();

            return _mapper.Map<WLMenuRolePermissionDto?>(entity);
        }




        public async Task SaveSelectedPermissionsAsync(string roleUUID, List<string> selectedPermissionUUIDs, List<string> displayedPermissionUUIDs, string userUuid, string ip)
        {
            if (string.IsNullOrWhiteSpace(roleUUID))
                throw new Exception("Role ID is required.");

            selectedPermissionUUIDs ??= new List<string>();
            displayedPermissionUUIDs ??= new List<string>();

            // Get ALL permissions (active + inactive) for this role so we can update existing inactive records
            var allRolePermissions = await GetAllByRoleIncludeInactiveAsync(roleUUID);

            // Build a lookup that handles duplicate PermissionUUIDs gracefully.
            // If duplicates exist, prefer the active record; otherwise take the first.
            var byPermission = allRolePermissions
                .Where(p => !string.IsNullOrWhiteSpace(p.PermissionUUID))
                .GroupBy(p => p.PermissionUUID, StringComparer.OrdinalIgnoreCase)
                .ToDictionary(
                    g => g.Key,
                    g => g.OrderByDescending(x => x.IsActive).First(), // prefer active
                    StringComparer.OrdinalIgnoreCase);

            // 1. ACTIVATE/CREATE selected (checked) permissions
            foreach (var permissionUUID in selectedPermissionUUIDs)
            {
                if (string.IsNullOrWhiteSpace(permissionUUID))
                    continue;

                if (byPermission.TryGetValue(permissionUUID, out var existing))
                {
                    if (!existing.IsActive)
                    {
                        // Update existing inactive record to active
                        var activateCommand = new WLMenuRolePermissionCommand
                        {
                            UUID = existing.UUID,
                            RoleUUID = roleUUID,
                            PermissionUUID = permissionUUID,
                            IsActive = true
                        };
                        await SaveAsync(activateCommand, userUuid, ip);
                    }
                    // if already active -> nothing to do
                }
                else
                {
                    // Create NEW permission record (no prior record)
                    var createCommand = new WLMenuRolePermissionCommand
                    {
                        RoleUUID = roleUUID,
                        PermissionUUID = permissionUUID,
                        IsActive = true
                    };
                    await SaveAsync(createCommand, userUuid, ip);
                }
            }

            // 2. DEACTIVATE ONLY permissions that are:
            //    - Currently active AND
            //    - Were DISPLAYED in the form AND
            //    - Were NOT checked (unchecked)
            var currentlyActivePermissions = byPermission.Values.Where(p => p.IsActive).ToList();

            foreach (var existing in currentlyActivePermissions)
            {
                if (displayedPermissionUUIDs.Contains(existing.PermissionUUID) &&
                    !selectedPermissionUUIDs.Contains(existing.PermissionUUID))
                {
                    var deactivateCommand = new WLMenuRolePermissionCommand
                    {
                        UUID = existing.UUID,
                        RoleUUID = roleUUID,
                        PermissionUUID = existing.PermissionUUID,
                        IsActive = false
                    };
                    await SaveAsync(deactivateCommand, userUuid, ip);
                }
            }
        }
        protected override Task<bool> IsDuplicateAsync(WLMenuRolePermissionCommand command)
            => Task.FromResult(false);

        protected override Expression<Func<WL_MenuRolePermission, bool>>? BuildSearchFilter(string searchTerm)
            => null;

        protected override Func<IQueryable<WL_MenuRolePermission>, IOrderedQueryable<WL_MenuRolePermission>>? BuildSortExpression(
            string? sortColumn,
            string? sortDirection)
            => null;
    }
}
    
