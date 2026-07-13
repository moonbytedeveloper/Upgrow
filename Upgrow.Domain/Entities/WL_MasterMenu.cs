using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Domain.Common;

namespace Upgrow.Domain.Entities
{
    public class WL_MasterMenu : BaseEntity
    {
 
        public string? MenuName { get; set; }
        public string? MenuIcon { get; set; }
        public decimal? MenuLevel { get; set; }
        public string? MainParentUUID { get; set; }
    
        public string? PermissionUUID { get; set; }
        public string? Url { get; set; }
        public bool IsParent { get; set; }
        public decimal? Sequence { get; set; }


    }
}
