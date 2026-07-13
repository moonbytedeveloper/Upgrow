using System;
using System.ComponentModel.DataAnnotations;
using VerifyIndia.Application.Commands.Master;

namespace VerifyIndia.Application.Commands.Support
{
    public class SupportTicketHeaderCommand : IMasterCommand
    {
        public string? UUID { get; set; }

        [Required(ErrorMessage = "Required!")]
        public string? TicketNumber { get; set; }
        public string? TicketCategoryUUID { get; set; }

        [Required(ErrorMessage = "Required!")]
        public string? UserType { get; set; }

        [Required(ErrorMessage = "Required!")]
        public string? UserUUID { get; set; }
        public string? Subject { get; set; }
        public string? Otp { get; set; }

        public DateTimeOffset? CreatedAt { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }

        public string? AssigneeUUID { get; set; }

        public bool IsActive { get; set; }
    }
}