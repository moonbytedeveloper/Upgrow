using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using Upgrow.Application.Commands.WL;
using Upgrow.Application.DTOs.Master;

namespace UpgrowAdminPanel.Models.Tenant
{
    public class WLMasterCmsVM
    {
        public string UUID { get; set; } = null!;

        [Required(ErrorMessage = "Required!")]
        public string PageTitle { get; set; } = null!;
        public string SelectedCmsUUID { get; set; } = null!; 
        [Required(ErrorMessage = "Required!")]
        public string UploadImage { get; set; } = null!;

        public string Description { get; set; }

        // Image Upload File
        public IFormFile? ImageFile { get; set; }

        // For Dropdown Options
        public List<(string UUID, string PageTitle)> CmsOptions { get; set; } = new();

        public bool IsActive { get; set; }
        public List<SelectListItem> Tenantlist { get; set; } = new();
        public List<SelectListItem> pagelist { get; set; } = new();
        public int TenantId { get; set; }

    }
}

