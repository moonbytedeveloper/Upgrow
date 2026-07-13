using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.DTO.DropDown;

namespace VerifyIndia.Application.Commands.Master
{
    public class MasterBlogCommand : IMasterCommand
    {
        public string? UUID { get; set; }

        [Required(ErrorMessage = "Required!")]
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Only letters are allowed")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "Required!")]
        public string? BlogCategoryUUID { get; set; }
        public string? BlogImageUrl { get; set; }
        [NotMapped]
        public string? BlogUrlwithdomain { get; set; }

        [NotMapped]
        public string? BannerUrlwithdomain { get; set; }

        public string? BannerImageUrl { get; set; }

        public string? ShortDescription { get; set; }
        [Required(ErrorMessage = "Required!")]
        public DateTime? BlogDate { get; set; }

        public string? FullDescription { get; set; }

        public bool IsActive { get; set; }

        //Image Fields
        public IFormFile? BlogImageFile { get; set; }
        public IFormFile? BannerImageFile { get; set; }
       
       
    }
}
