using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.Website
{
    public class Website_VerificationServiceDto
    {
        public string? UUID { get; set; }
        public string? Title { get; set; }
        public string? ServiceCategoryUUID { get; set; }
        public bool IsActive { get; set; }
    }
}
