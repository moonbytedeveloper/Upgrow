using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Domain.Common;

namespace Upgrow.Domain.Entities.WL.Master
{
    public class WL_MasterFinancialYear: TenantEntity
    {
       
        public string? Title { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        
    }
}
