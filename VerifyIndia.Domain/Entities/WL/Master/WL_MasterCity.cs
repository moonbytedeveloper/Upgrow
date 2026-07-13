using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Domain.Entities
{
    public class WL_MasterCity : TenantEntity
    {
        public string CountryUUID { get; set; }
        public string StateUUID { get; set; }
        public string? Title { get; set; }
        public string? ShortTitle { get; set; }
    }
}
