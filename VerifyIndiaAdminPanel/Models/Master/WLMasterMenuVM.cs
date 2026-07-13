using Microsoft.AspNetCore.Mvc.Rendering;
using VerifyIndia.Application.Commands.Master;

namespace UpgrowAdminPanel.Models.Master
{
    public class WLMasterMenuVM
    {
        public WLMasterMenuCommand MasterMenu { get; set; } = new WLMasterMenuCommand();

        public IEnumerable<SelectListItem> MenuLevelList { get; set; } = new List<SelectListItem>();

        public IEnumerable<SelectListItem> ParentMenuList { get; set; } = new List<SelectListItem>();

        public IEnumerable<SelectListItem> SubMenuList { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> PermissionList { get; set; } = new List<SelectListItem>();
    }
}
