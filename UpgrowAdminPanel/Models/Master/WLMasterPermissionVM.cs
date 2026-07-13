using Microsoft.AspNetCore.Mvc.Rendering;
using Upgrow.Application.Commands.Master;

namespace UpgrowAdminPanel.Models.Master
{
    public class WLMasterPermissionVM
    {
        public WLMasterPermissionCommand Permission { get; set; } = new();
        public List<SelectListItem> PermissionGroupList { get; set; } = new();
    }
}
