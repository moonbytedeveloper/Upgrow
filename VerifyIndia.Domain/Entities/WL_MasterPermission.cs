using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Domain.Entities
{
    public class WL_MasterPermission : BaseEntity
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? PermissionGroupUUID { get; set; }
    }
}
