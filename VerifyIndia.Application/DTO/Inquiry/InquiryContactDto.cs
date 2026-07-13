using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Commands.Master;

namespace Upgrow.Application.DTO.Inquiry
{
    public class InquiryContactDto
    {
        public string? UUID { get; set; }
        public string? UserName { get; set; }
        public string? MobileNo { get; set; }
        public string? EmailId { get; set; }
        public string? Message { get; set; }
        public string? Status { get; set; }
    }
}
