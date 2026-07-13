using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.WL
{
    public class WLMasterCompanyBasicDataDto
    {
        public int TenantId { get; set; }
        public string? UUID { get; set; }
        public string? CompName { get; set; }
        public string? Phone { get; set; }
        public string? EmailId { get; set; }
        public string? GoogleMapIframe { get; set; }
        public string? Address { get; set; }
        public string? WebsiteLogo { get; set; }
        public string? StickyLogo { get; set; }
        public string? FooterLogo { get; set; }
        public string? TenantName { get; set; }
        public bool IsActive { get; set; }
    }
}
