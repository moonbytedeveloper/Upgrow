using System;

namespace VerifyIndia.Application.DTO.Support
{
    public class SupportTicketDto
    {
        public string UUID { get; set; } = null!;
        public string TicketNumber { get; set; } = null!;
        public string Subject { get; set; } = null!;
        public string Message { get; set; } = null!;
        public string Otp { get; set; } = null!;
        public string TicketCategoryUUID { get; set; } = null!;
        public string UserType { get; set; } = null!;
        public string UserUUID { get; set; } = null!;
        public DateTimeOffset CreatedAt { get; set; } 
        public DateTimeOffset? UpdateOn { get; set; } 
        public string? AssigneeUUID { get; set; }
        public bool IsActive { get; set; }

        public string? RaisedByName { get; set; }
        public string? RaisedByMobile { get; set; }
        public string? RaisedByEmail { get; set; }
    }
}