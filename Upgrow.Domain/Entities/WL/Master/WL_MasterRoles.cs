using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Domain.Entities.WL.Master
{
    public class WL_MasterRoles : TenantEntity
    {
        public string Title { get; set; }
        public string ShortTitle { get; set; }
    }
}
