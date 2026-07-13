using Microsoft.AspNetCore.Mvc.Rendering;
using VerifyIndia.Application.Commands.Api;

namespace UpgrowAdminPanel.Models.Api
{
    public class ApiInfoSectionVM
    {
        public ApiInfoSectionCommand ApiInfoSection { get; set; } = new();
        public IEnumerable<SelectListItem> Api { get; set; } = [];
        
    }
}
