using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.CustomerPanel
{
    public class NewsCategoryApiDto
    {
        public string UUID { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string? ShortDescription { get; set; }
        public DateTime? PublishDate { get; set; }
        public bool IsActive { get; set; }
    }
}
