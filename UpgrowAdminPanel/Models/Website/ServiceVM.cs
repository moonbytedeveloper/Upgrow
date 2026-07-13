using Microsoft.AspNetCore.Mvc.Rendering;
using Upgrow.Application.Commands.Master;
using Upgrow.Application.Commands.Website;

namespace UpgrowAdminPanel.Models.Website
{
    public class ServiceVM
    {
        public Website_VerificationServiceCommand Service { get; set; }
        public List<SelectListItem> ServiceCategoryList { get; set; } = new();

    }
}
