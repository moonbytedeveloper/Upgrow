using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Domain.Entities
{
    public class Master_PathPermission : BaseEntity
    {
        public string PathUUID { get; set; }
        public string? PermissionUUID { get; set; }
    }
}
