using Microsoft.AspNetCore.Mvc.Rendering;
using VerifyIndia.Application.Commands.Master;

namespace VerifyIndiaAdminPanel.Models.Master
{
    public class MasterPermissionVM
    {
        public MasterPermissionCommand Permission { get; set; } = new();
        public List<SelectListItem> PermissionGroupList { get; set; } = new();
    }
}
