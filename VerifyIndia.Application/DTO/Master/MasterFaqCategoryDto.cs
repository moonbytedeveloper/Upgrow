using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.Master
{
    public class MasterFaqCategoryDto
    {
        public string? UUID { get; set; }
        public string? Title { get; set; }
        public string? Icon { get; set; }
        public string? ShortDescription { get; set; }
        public bool IsActive { get; set; }
    }
}
