using Microsoft.AspNetCore.Mvc.Rendering;
using Upgrow.Application.Commands.Master;

namespace UpgrowAdminPanel.Models.Master
{
    public class MasterFaqSubCategoryVM
    {
        public MasterFaqSubCategoryCommand FaqSubCategory { get; set; }
        public List<SelectListItem> FaqCategoryList { get; set; } = new();
    }
}
