using Microsoft.AspNetCore.Mvc.Rendering;
using Upgrow.Application.Commands.Support;
using Upgrow.Application.DTO.Support;

namespace UpgrowAdminPanel.Models.Support
{
    public class SupportTicketsVM
    {
        public List<SelectListItem> UserTypeList { get; set; } = new List<SelectListItem>
         {
          new SelectListItem { Text = "White Label", Value = "WhiteLabel" },
          new SelectListItem { Text = "Customer", Value = "Customer" },
          new SelectListItem { Text = "Agent", Value = "Agent" }
         };
        public List<SelectListItem> UserList { get; set; } = new();
        public List<SelectListItem> CategoryList { get; set; } = new();
        public SupportTicketHeaderCommand? SupportTicket { get; set; }
        public SupportTicketLineAttachmentCommand? SupportTicketAttachment { get; set; }
        public SupportTicketLineDto? SupportTicketLine { get; set; }
    }
}