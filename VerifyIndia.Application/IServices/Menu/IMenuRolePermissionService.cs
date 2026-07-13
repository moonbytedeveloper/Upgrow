using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Commands.Menu;
using VerifyIndia.Application.DTO.Menu;
using VerifyIndia.Application.IServices.Master;

namespace VerifyIndia.Application.IServices.Menu
{
    public interface IMenuRolePermissionService : IMasterService<MenuRolePermissionDto, MenuRolePermissionCommand>
    {
        Task<List<MenuRolePermissionDto>> GetAllActiveAsync();
        Task<List<MenuRolePermissionDto>> GetAllByRoleAsync(string roleUUID);
        Task<MenuRolePermissionDto> GetByRoleAndPermissionAsync(string roleUUID, string permissionUUID);
        /// <summary>
        /// Get ALL permissions for a role (both active and inactive)
        /// </summary>
        Task<List<MenuRolePermissionDto>> GetAllByRoleIncludeInactiveAsync(string roleUUID);
        /// <summary>
        /// Save selected permissions for a role
        /// Only deactivates permissions that were displayed in the form
        /// </summary>
        Task SaveSelectedPermissionsAsync(string roleUUID, List<string> selectedPermissionUUIDs, List<string> displayedPermissionUUIDs, string userUuid, string ip);
    }
}

