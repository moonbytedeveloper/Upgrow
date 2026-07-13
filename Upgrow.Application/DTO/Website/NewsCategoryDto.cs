using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO
{
    public class NewsCategoryDto
    {
        public string UUID { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string? ShortDescription { get; set; }
        public bool IsActive { get; set; }
    }
}
