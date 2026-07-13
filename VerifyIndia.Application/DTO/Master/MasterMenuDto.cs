using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTO.Master
{
    public class MasterMenuDto
    {
        public string? UUID { get; set; }
        public string? MenuName { get; set; }
        public string? MenuIcon { get; set; }
        public decimal? MenuLevel { get; set; }
        public string? MainParentUUID { get; set; }
        public string? SubParentUUID { get; set; }
        public string? PermissionUUID { get; set; }
        public string? Url { get; set; }
        public bool IsParent { get; set; }
        public bool IsActive { get; set; }
        public decimal? Sequence { get; set; }
        public List<MasterMenuDto> Children { get; set; } = new();
    }
}
