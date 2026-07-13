using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.Support
{
    public class SupportTicketCategoryDto
    {
        public string UUID { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string DesignationUUID { get; set; } = null!;
        public string UserType { get; set; } = null!;
        
        public bool IsActive { get; set; }
    }
}
