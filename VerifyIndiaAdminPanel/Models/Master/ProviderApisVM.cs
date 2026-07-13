using Microsoft.AspNetCore.Mvc.Rendering;
using VerifyIndia.Application.Commands.Master;

namespace UpgrowAdminPanel.Models.Master
{
    public class ProviderApisVM
    {
        public ProviderApisCommand ProviderApi { get; set; }
        public List<SelectListItem> ApiList { get; set; } = new();
        public List<SelectListItem> ProviderList { get; set; } = new();
    }
}
