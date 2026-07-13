using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Domain.Entities
{
    public class WL_MasterCompanyBasicData :TenantEntity
    {
        public string? CompName { get; set; }
        public string? Phone { get; set; }
        public string? EmailId { get; set; }
        public string? GoogleMapIframe { get; set; }
        public string? Address { get; set; }
        public string? WebsiteLogo { get; set; }
        public string? StickyLogo { get; set; }
        public string? FooterLogo { get; set; }
        [NotMapped]
        public string? TenantName { get; set; }
    }
}
