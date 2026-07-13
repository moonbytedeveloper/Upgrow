using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Commands.Master;

namespace VerifyIndia.Application.Commands.Website
{
    public class NewsCommand : IMasterCommand
    {
        public string? UUID { get; set; }

        [Required(ErrorMessage = "Required!")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "Required!")]
        public string? NewsCategoryUUID { get; set; }
        public string? Image { get; set; }

        public string? ShortDescription { get; set; }
        [Required(ErrorMessage = "Required!")]
        public string? FullDescription { get; set; }

        public bool IsActive { get; set; }
        public bool IsTopStory { get; set; }
        [Required(ErrorMessage = "Required!")]
        public string? Location { get; set; }
        [Required(ErrorMessage = "Required!")]
        public string? Source { get; set; }
        
        public string? CardImage { get; set; }
        [Required(ErrorMessage = "Required!")]
        public DateTime? PublishDate { get; set; }
        [NotMapped]
        public string? ImageUrlwithdomain { get; set; }
        [NotMapped]
        public string? CardImageUrlwithdomain { get; set; }
        //Image Fields
        public IFormFile? ImageFile { get; set; }
        public IFormFile? CardImageFile { get; set; }
    }
}
