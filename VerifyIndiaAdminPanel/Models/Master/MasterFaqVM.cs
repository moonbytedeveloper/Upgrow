using Microsoft.AspNetCore.Mvc.Rendering;
using VerifyIndia.Application.Commands.Master;

namespace UpgrowAdminPanel.Models.Master
{
    public class MasterFaqVM
    {
        public MasterFaqCommand Faq { get; set; }
        public List<SelectListItem> FaqCategoryList { get; set; } = new();
        public List<SelectListItem> FaqSubCategoryList { get; set; } = new();
     
        
    }

}
