using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.Commands.Support
{
    public class SupportTicketLineAttachmentCommand
    {
        public string ?UUID { get; set; }
        public string? LineUUID { get; set; }
        public List<IFormFile>? Image { get; set; }
        public string? URL { get; set; }
    }
}
