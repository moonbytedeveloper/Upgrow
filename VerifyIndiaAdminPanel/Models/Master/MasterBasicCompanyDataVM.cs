using System.ComponentModel.DataAnnotations.Schema;

namespace UpgrowAdminPanel.Models.Master
{
    public class MasterBasicCompanyDataVM
    {
        public string? UUID { get; set; }
        public string? CompName { get; set; }
        public string? Phone { get; set; }
        public string? EmailId { get; set; }
        public string? GoogleMapIframe { get; set; }
        public string? Address { get; set; }
        public string? WebsiteLogo { get; set; }
        public string? StickyLogo { get; set; }
        public string? FooterLogo { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsIncremental { get; set; } = false;

        public IFormFile? WebsiteLogoImage { get; set; }
        public IFormFile? StickyLogoImage { get; set; }
        public IFormFile? FooterLogoImage { get; set; }
    }
}
