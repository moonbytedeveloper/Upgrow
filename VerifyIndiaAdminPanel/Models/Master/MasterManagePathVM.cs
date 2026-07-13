using Microsoft.AspNetCore.Mvc.Rendering;
using VerifyIndia.Application.Commands.Master;

namespace VerifyIndiaAdminPanel.Models.Master
{
    public class MasterManagePathVM
    {
        public MasterPathPermissionCommand ManagePath { get; set; } = new();
        public List<SelectListItem> PathList { get; set; } = new();
        public List<SelectListItem> PermissionList { get; set; } = new();
    }
}
