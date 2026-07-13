using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.Commands.Master
{
    public class MasterFaqSubCategoryCommand : IMasterCommand
    {
        public string? UUID { get; set; }

        [Required(ErrorMessage = "Required!")]
        public string? Title { get; set; }

        public List<SelectListItem> FaqCategoryList { get; set; } = new();

        public string? Image { get; set; }

        public IFormFile?  ImageFile { get; set; }

        [NotMapped]
        public string? ImageUrlwithdomain { get; set; }

        [Required(ErrorMessage = "Required!")]
        public string? FAQCategoryUUID { get; set; }
        public bool IsActive { get; set; }
    }
}
