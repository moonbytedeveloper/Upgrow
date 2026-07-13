using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Commands.Master;

namespace Upgrow.Application.Commands.WL
{
    public class WLMasterCompanyBasicDataCommand :IMasterCommand
    {
        [Required(ErrorMessage = "Required!")]
        public int? TenantId { get; set; }
        public string? TenantName { get; set; }
        public string? UUID { get; set; }

        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Only letters are allowed")]
        public string? CompName { get; set; }
        [Required(ErrorMessage = "Required!")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be exactly 10 digits")]
        public string? Phone { get; set; }

        [Required(ErrorMessage = "Required!")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string? EmailId { get; set; }
        public string? GoogleMapIframe { get; set; }
        public string? Address { get; set; }
        public string? WebsiteLogo { get; set; }
        public string? StickyLogo { get; set; }
        public string? FooterLogo { get; set; }
        public IFormFile? WebsiteLogoImage { get; set; }
        public IFormFile? StickyLogoImage { get; set; }
        public IFormFile? FooterLogoImage { get; set; }
        [NotMapped]
        public string? websiteUrlwithdomain { get; set; }

        [NotMapped]
        public string? stickylogoUrlwithdomain { get; set; }

        [NotMapped]
        public string? footerUrlwithdomain { get; set; }
        public List<SelectListItem> Tenants { get; set; } = new List<SelectListItem>();

        public bool IsActive { get; set; }
    }
}
