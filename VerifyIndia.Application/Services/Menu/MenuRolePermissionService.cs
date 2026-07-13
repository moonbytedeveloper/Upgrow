using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Commands.Menu;
using VerifyIndia.Application.DTO.Menu;
using VerifyIndia.Application.IServices.Menu;
using VerifyIndia.Application.Services.Master;
using VerifyIndia.Domain.Common;
using VerifyIndia.Domain.Entities;
using VerifyIndia.Domain.IRepositories;

namespace VerifyIndia.Application.Services.Menu
{
    public class MenuRolePermissionService : MasterServiceBase<Menu_RolePermission, MenuRolePermissionDto, MenuRolePermissionCommand>, IMenuRolePermissionService
    {
        public MenuRolePermissionService(IMasterRepository<Menu_RolePermission> repository, IMapper mapper)
            : base(repository, mapper) { }

        public async Task<List<MenuRolePermissionDto>> GetAllActiveAsync()
        {
            var entities = await _repository.GetAllActiveAsync();
            return _mapper.Map<List<MenuRolePermissionDto>>(entities);
        }

        public async Task<List<MenuRolePermissionDto>> GetAllByRoleAsync(string roleUUID)
        {
            if (string.IsNullOrWhiteSpace(roleUUID))
                return new List<MenuRolePermissionDto>();

            Expression<Func<Menu_RolePermission, bool>> filter = x => x.RoleUUID == roleUUID && x.IsActive;
            var pagination = new PaginationParams { PageNumber = 1, PageSize = int.MaxValue };

            var result = await _repository.GetPagedAsync(filter, pagination);
            return _mapper.Map<List<MenuRolePermissionDto>>(result.Items);
        }
        

        /// <summary>
        /// Get ALL permissions for a role (active AND inactive)
        /// </summary>
        public async Task<List<MenuRolePermissionDto>> GetAllByRoleIncludeInactiveAsync(string roleUUID)
        {
            if (string.IsNullOrWhiteSpace(roleUUID))
                return new List<MenuRolePermissionDto>();

            //  get ALL records
            Expression<Func<Menu_RolePermission, bool>> filter = x => x.RoleUUID == roleUUID;
            var pagination = new PaginationParams { PageNumber = 1, PageSize = int.MaxValue };

            var result = await _repository.GetPagedAsync(filter, pagination);
            return _mapper.Map<List<MenuRolePermissionDto>>(result.Items);
        }

        public async Task<MenuRolePermissionDto?> GetByRoleAndPermissionAsync(string roleUUID, string permissionUUID)
        {
            if (string.IsNullOrWhiteSpace(roleUUID) || string.IsNullOrWhiteSpace(permissionUUID))
                return null;

            Expression<Func<Menu_RolePermission, bool>> filter = x =>
                x.RoleUUID == roleUUID && x.PermissionUUID == permissionUUID;

            var pagination = new PaginationParams { PageNumber = 1, PageSize = 1 };

            var result = await _repository.GetPagedAsync(filter, pagination);
            var entity = result.Items.FirstOrDefault();

            return _mapper.Map<MenuRolePermissionDto?>(entity);
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
                        var activateCommand = new MenuRolePermissionCommand
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
                    var createCommand = new MenuRolePermissionCommand
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
                    var deactivateCommand = new MenuRolePermissionCommand
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
        protected override Task<bool> IsDuplicateAsync(MenuRolePermissionCommand command)
            => Task.FromResult(false);

        protected override Expression<Func<Menu_RolePermission, bool>>? BuildSearchFilter(string searchTerm)
            => null;

        protected override Func<IQueryable<Menu_RolePermission>, IOrderedQueryable<Menu_RolePermission>>? BuildSortExpression(
            string? sortColumn,
            string? sortDirection)
            => null;
    }
}