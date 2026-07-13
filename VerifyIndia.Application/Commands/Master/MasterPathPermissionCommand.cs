using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.Commands.Master
{
    public class MasterPathPermissionCommand : IMasterCommand
    {
        public string? UUID { get; set; }
        public string PathUUID { get; set; }
        public string PermissionUUID { get; set; }
        public bool IsActive { get; set; } = true; // Set default to true
    }
}
