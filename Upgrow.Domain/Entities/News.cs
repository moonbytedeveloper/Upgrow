using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Domain.Common;

namespace Upgrow.Domain.Entities
{
    public class News : BaseEntity
    {
 
        public string? Title { get; set; }
        public string? NewsCategoryUUID { get; set; }       
        public string? ShortDescription { get; set; }
        public string? FullDescription { get; set; }
        public string? Source { get; set; }
        public string? Location { get; set; }
        public string? CardImage { get; set; }
        public string? Image { get; set; }
        public DateTime? PublishDate { get; set; }
          public DateTime? CreatedAt { get; set; }
 
        public bool IsTopStory { get; set; }
    }
}
