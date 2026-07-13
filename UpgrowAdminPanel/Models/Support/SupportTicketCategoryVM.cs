using Microsoft.AspNetCore.Mvc.Rendering;
using Upgrow.Application.Commands.Master;
using Upgrow.Application.Commands.Support;

namespace UpgrowAdminPanel.Models.Support
{
    public class SupportTicketCategoryVM
    {
        public SupportTicketCategoryCommand STC { get; set; } = new();
        public List<SelectListItem> DesignationList { get; set; } = new();
        public List<SelectListItem> UserTypeList { get; set; } = new();
    }
}
