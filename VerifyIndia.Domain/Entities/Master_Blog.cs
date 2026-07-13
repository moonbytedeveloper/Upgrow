using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Domain.Entities;

namespace VerifyIndia.Domain.Entities
{
    public class Master_Blog : BaseEntity
    {    
        public string? Title { get; set; }

        public string? BlogCategoryUUID { get; set; }

        public string? BlogImageUrl { get; set; }

        public string? BannerImageUrl { get; set; }

        public string? ShortDescription { get; set; }

        public DateTime? BlogDate { get; set; }

        public string? FullDescription { get; set; }     
    }
}
