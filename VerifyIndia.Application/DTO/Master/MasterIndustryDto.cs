using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.Master
{
    public class MasterIndustryDto
    {
        public string UUID { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Icon { get; set; } = null!;
        public string? Image { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }
}
