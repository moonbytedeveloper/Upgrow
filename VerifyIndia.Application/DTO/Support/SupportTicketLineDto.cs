using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.Support
{
    public class SupportTicketLineDto
    {
        public string?UUID { get; set; }
        public string? HeaderUUID { get; set; }
        public string? UserUUID { get; set; }
        public string? Message { get; set; }
        public bool IsActive { get; set; } = true;

    }
}
