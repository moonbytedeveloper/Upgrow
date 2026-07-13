using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Domain.Entities.WL.Master
{
    public class WL_MenuRolePermission : TenantEntity
    {
        public string RoleUUID { get; set; }
        public string PermissionUUID { get; set; }
    }
}
