using Microsoft.AspNetCore.Mvc.Rendering;
using Upgrow.Application.Commands.Master;
using Upgrow.Application.Commands.WL;

namespace UpgrowAdminPanel.Models.WL
{
    public class TenantDomainVM
    {
        public WLTenantDomainCommand TenantDomain { get; set; }
        public List<SelectListItem> TenantList { get; set; } = new();
    }
}
