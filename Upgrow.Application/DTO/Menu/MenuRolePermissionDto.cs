using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.Menu
{
    public class MenuRolePermissionDto
    {
        public string UUID { get; set; } = null!;
        public string RoleUUID { get; set; } = null!;
        public string PermissionUUID { get; set; } = null!;
        public bool IsActive { get; set; }
    }
    public class PermissionGroupWithPermissionsDto
    {
        public string GroupUUID { get; set; }
        public string GroupTitle { get; set; }
        public List<PermissionWithStatusDto> Permissions { get; set; } = new();
    }

    public class PermissionWithStatusDto
    {
        public string PermissionUUID { get; set; }
        public string PermissionName { get; set; }
        public string PermissionDescription { get; set; }
        public bool IsAssigned { get; set; }
    }
}
