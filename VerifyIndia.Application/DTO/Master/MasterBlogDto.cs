using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTOs.Master
{
    public class MasterBlogDto
    {
        public string UUID { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string? BlogCategoryUUID { get; set; }        
        public string? BlogImageUrl { get; set; }
        public string? BannerImageUrl { get; set; }
        public string? ShortDescription { get; set; }
        public DateTime? BlogDate { get; set; }
        public string? FullDescription { get; set; }
        public bool IsActive { get; set; }
    }
}
