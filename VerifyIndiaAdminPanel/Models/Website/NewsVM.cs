using Microsoft.AspNetCore.Mvc.Rendering;
using VerifyIndia.Application.Commands.Master;
using VerifyIndia.Application.Commands.Website;

namespace VerifyIndiaAdminPanel.Models.Website
{
    public class NewsVM
    {
        public NewsCommand News { get; set; } = new();
        public List<SelectListItem> CategoryList { get; set; } = new();
    }
}
