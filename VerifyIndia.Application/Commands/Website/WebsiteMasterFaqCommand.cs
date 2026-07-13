using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Commands.Master;

namespace VerifyIndia.Application.Commands.Website
{
    public class WebsiteMasterFaqCommand : IMasterCommand
    {
        public string? UUID { get; set; }
        [Required(ErrorMessage = "Required!")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "Required!")]
        public string? FAQCategoryUUID { get; set; }
        [Required(ErrorMessage = "Required!")]
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public bool IsFeatured { get; set; }
    }
}
