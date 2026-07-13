using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Commands.Menu;
using Upgrow.Application.Commands.WL.Master;
using Upgrow.Application.DTO.Menu;
using Upgrow.Application.DTO.WL.Master;
using Upgrow.Application.IServices.Master;

namespace Upgrow.Application.IServices.WL.Master
{
    public interface IWLMenuRolePermissionService : IMasterService<WLMenuRolePermissionDto, WLMenuRolePermissionCommand>
    {
        Task<List<WLMenuRolePermissionDto>> GetAllActiveAsync();
        Task<List<WLMenuRolePermissionDto>> GetAllByRoleAsync(string roleUUID);
        Task<WLMenuRolePermissionDto> GetByRoleAndPermissionAsync(string roleUUID, string permissionUUID);
        /// <summary>
        /// Get ALL permissions for a role (both active and inactive)
        /// </summary>
        Task<List<WLMenuRolePermissionDto>> GetAllByRoleIncludeInactiveAsync(string roleUUID);
        /// <summary>
        /// Save selected permissions for a role
        /// Only deactivates permissions that were displayed in the form
        /// </summary>
        Task SaveSelectedPermissionsAsync(string roleUUID, List<string> selectedPermissionUUIDs, List<string> displayedPermissionUUIDs, string userUuid, string ip);
    }
}
