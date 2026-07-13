using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Domain.Entities.WL.Master
{
    public class WL_MasterNomenClature : TenantEntity
    {
        public string? ModuleKey { get; set; }
        public bool? IsIncludeYear { get; set; }
        public string? Prefix { get; set; }
        public decimal? StartNo { get; set; }
        public string? FinancialYearUUID { get; set; }
        public int? NumberOfDigits { get; set; }
    }
}
