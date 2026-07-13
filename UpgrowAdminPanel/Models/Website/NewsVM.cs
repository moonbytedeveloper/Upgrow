using Microsoft.AspNetCore.Mvc.Rendering;
using Upgrow.Application.Commands.Master;
using Upgrow.Application.Commands.Website;

namespace UpgrowAdminPanel.Models.Website
{
    public class NewsVM
    {
        public NewsCommand News { get; set; } = new();
        public List<SelectListItem> CategoryList { get; set; } = new();
    }
}
