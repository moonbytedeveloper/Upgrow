using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.Support
{
    public class TicketConversationDto
    {
        public string LineUUID { get; set; }
        public string UserUUID { get; set; }
        public string UserType { get; set; }
        public string UserName { get; set; }
        public string Message { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public List<string> Url { get; set; } = new();
    }
}
