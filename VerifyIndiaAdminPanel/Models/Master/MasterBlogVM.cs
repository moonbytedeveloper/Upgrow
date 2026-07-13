using Microsoft.AspNetCore.Mvc.Rendering;
using VerifyIndia.Application.Commands.Master;

namespace UpgrowAdminPanel.Models.Master
{
    public class MasterBlogVM
    {
        public MasterBlogCommand Command { get; set; } = new();
        public IEnumerable<SelectListItem> BlogCategories { get; set; } = [];
    }
}
