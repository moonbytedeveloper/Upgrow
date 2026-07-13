using Microsoft.AspNetCore.Mvc.Rendering;

namespace VerifyIndiaAdminPanel.Models.WL
{
    public class ClientsVM
    {
        public List<SelectListItem> Tenants { get; set; } = new List<SelectListItem>();

        // Optional: store selected value if needed for initial selection
        public string? SelectedTenantValue { get; set; }
    }
}
