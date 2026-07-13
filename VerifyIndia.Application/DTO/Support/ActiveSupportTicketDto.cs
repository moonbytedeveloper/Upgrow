using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.Support
{
    public class ActiveSupportTicketDto
    {
        public string UUID { get; set; }
        public string TicketNumber { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public bool IsActive { get; set; }  // "Open" or "Closed"
        public string Otp { get; set; }  
        public string Category { get; set; }  
        public string AssignedTo { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }  
    }
}
