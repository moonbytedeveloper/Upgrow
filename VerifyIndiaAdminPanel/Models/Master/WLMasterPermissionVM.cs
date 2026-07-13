using Microsoft.AspNetCore.Mvc.Rendering;
using VerifyIndia.Application.Commands.Master;

namespace VerifyIndiaAdminPanel.Models.Master
{
    public class WLMasterPermissionVM
    {
        public WLMasterPermissionCommand Permission { get; set; } = new();
        public List<SelectListItem> PermissionGroupList { get; set; } = new();
    }
}
