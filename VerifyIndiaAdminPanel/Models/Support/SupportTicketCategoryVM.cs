using Microsoft.AspNetCore.Mvc.Rendering;
using VerifyIndia.Application.Commands.Master;
using VerifyIndia.Application.Commands.Support;

namespace UpgrowAdminPanel.Models.Support
{
    public class SupportTicketCategoryVM
    {
        public SupportTicketCategoryCommand STC { get; set; } = new();
        public List<SelectListItem> DesignationList { get; set; } = new();
        public List<SelectListItem> UserTypeList { get; set; } = new();
    }
}
