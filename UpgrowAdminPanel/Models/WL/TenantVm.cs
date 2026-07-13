using Microsoft.AspNetCore.Mvc.Rendering;

namespace UpgrowAdminPanel.Models.WL
{
    public class TenantVm
    {
        public List<SelectListItem> Tenants { get; set; } = new List<SelectListItem>();
        public string? SelectedTenantValue { get; set; }
    }
}
