using Microsoft.AspNetCore.Mvc.Rendering;
using VerifyIndia.Application.Commands.Master;

namespace UpgrowAdminPanel.Models.Master
{
    public class MasterCityVM
    {
        public MasterCityCommand City { get; set; } = new();
        public List<SelectListItem> CountryList { get; set; } = new();
        public List<SelectListItem> StateList { get; set; } = new();
    }
}
