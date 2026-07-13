using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTO.Master
{
    public class MasterPermissionDto
    {
        public string UUID { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string? PermissionGroupUUID { get; set; }       
        public bool IsActive { get; set; }
    }
}

