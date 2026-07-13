using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Domain.Entities
{
    public class Menu_RolePermission : BaseEntity
    {
        public string RoleUUID { get; set; }
        public string PermissionUUID { get; set; }
    }
}
