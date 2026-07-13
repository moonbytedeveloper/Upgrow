using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Domain.Entities
{
    public class Master_Permission : BaseEntity
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? PermissionGroupUUID { get; set; }
    }
}
