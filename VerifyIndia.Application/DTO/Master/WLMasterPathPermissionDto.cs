using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTO.Master
{
    public class WLMasterPathPermissionDto
    {
        public string UUID { get; set; }
        public string PathUUID { get; set; }
        public string PermissionUUID { get; set; }
        public bool IsActive { get; set; }
    }
}
