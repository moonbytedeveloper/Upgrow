using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.CustomerPanel
{
    public class NewsApiDto
    {
        public string UUID { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string? NewsCategoryUUID { get; set; }
        public string? ShortDescription { get; set; }
        public string? Source { get; set; }
        public string? Location { get; set; }
        public string? Image { get; set; }
        public string? CardImage { get; set; }
        public string? FullDescription { get; set; }
        public DateTime? PublishDate { get; set; }
        public bool IsActive { get; set; }
        public bool? IsBookmarked { get; set; }
    }
}
