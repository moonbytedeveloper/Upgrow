using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Domain.Entities.Inquiry
{
    public class Inquiry_Career : BaseEntity
    {
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNo { get; set; }
        public string? JobPosition { get; set; }
        public string? Experience { get; set; }
        public string? Qualification { get; set; }
        public string? Message { get; set; }
        public string? Resume { get; set; }
        public string? Remark { get; set; }
        public string? ActionTakenBy { get; set; }

    }
}
