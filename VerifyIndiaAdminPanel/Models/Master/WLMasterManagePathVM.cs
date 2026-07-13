using Microsoft.AspNetCore.Mvc.Rendering;
using VerifyIndia.Application.Commands.Master;

namespace VerifyIndiaAdminPanel.Models.Master
{
    public class WLMasterManagePathVM
    {
        public WLMasterPathPermissionCommand ManagePath { get; set; } = new();
        public List<SelectListItem> PathList { get; set; } = new();
        public List<SelectListItem> PermissionList { get; set; } = new();
    }
}
