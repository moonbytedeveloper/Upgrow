using Microsoft.AspNetCore.Mvc.Rendering;
using VerifyIndia.Application.Commands.Master;
using VerifyIndia.Application.Commands.WL;

namespace UpgrowAdminPanel.Models.WL
{
    public class TenantDomainVM
    {
        public WLTenantDomainCommand TenantDomain { get; set; }
        public List<SelectListItem> TenantList { get; set; } = new();
    }
}
