using Microsoft.AspNetCore.Mvc.Rendering;
using Upgrow.Application.Commands.Master;
using Upgrow.Application.Commands.Website;

namespace UpgrowAdminPanel.Models.Website
{
    public class FaqVM
    {
        public WebsiteMasterFaqCommand Faq { get; set; }
        public List<SelectListItem> FaqCategoryList { get; set; } = new();

    }
}
