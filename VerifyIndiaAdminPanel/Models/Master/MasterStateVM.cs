using Microsoft.AspNetCore.Mvc.Rendering;
using VerifyIndia.Application.Commands.Master;

namespace UpgrowAdminPanel.Models.Master
{
    public class MasterStateVM
    {
        public MasterStateCommand State { get; set; } = new();
        public List<SelectListItem> CountryList { get; set; } = new();
    }
}
