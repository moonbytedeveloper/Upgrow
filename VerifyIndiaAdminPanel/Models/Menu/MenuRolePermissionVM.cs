using Microsoft.AspNetCore.Mvc.Rendering;
using VerifyIndia.Application.Commands.Menu;

namespace UpgrowAdminPanel.Models.Menu
{
    public class MenuRolePermissionVM
    {
        public MenuRolePermissionCommand RolePermission { get; set; } = new MenuRolePermissionCommand();
        public List<SelectListItem> RoleList { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> PermissionList { get; set; } = new List<SelectListItem>();
    }
}
