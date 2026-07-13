using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTO
{
    public class NewsDto
    {
        public string UUID { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string? NewsCategoryUUID { get; set; }
        public string? ShortDescription { get; set; }
        public string? FullDescription { get; set; }
     //   public DateTime? CreatedAt { get; set; }
        public string? Image { get; set; }
        public string? Source { get; set; }
        public string? Location { get; set; }
        public DateTime? PublishDate { get; set; }
        public string? CardImage { get; set; }
        public bool IsActive { get; set; }
        public bool IsTopStory { get; set; }

        public bool IsBookMarked { get; set; } = true;
    }

}
