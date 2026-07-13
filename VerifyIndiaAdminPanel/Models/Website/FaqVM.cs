using Microsoft.AspNetCore.Mvc.Rendering;
using VerifyIndia.Application.Commands.Master;
using VerifyIndia.Application.Commands.Website;

namespace VerifyIndiaAdminPanel.Models.Website
{
    public class FaqVM
    {
        public WebsiteMasterFaqCommand Faq { get; set; }
        public List<SelectListItem> FaqCategoryList { get; set; } = new();

    }
}
