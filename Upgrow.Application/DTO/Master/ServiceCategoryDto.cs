using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.Master
{
    public class ServiceCategoryDto
    {
        public string UUID { get; set; } = null!;
        public string CategoryName { get; set; }
        public string IconImage { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
}
