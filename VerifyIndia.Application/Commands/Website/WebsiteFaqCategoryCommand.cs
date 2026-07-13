using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Commands.Master;

namespace VerifyIndia.Application.Commands.Website
{
    public class WebsiteFaqCategoryCommand : IMasterCommand
    {
        public string? UUID { get; set; }

        [Required(ErrorMessage = "Required!")]
        public string? Title { get; set; }
        [Required(ErrorMessage = "Required!")]
        public string? Icon { get; set; }
        [Required(ErrorMessage = "Required!")]
        public string? ShortDescription { get; set; }
        public bool IsActive { get; set; }
    }
}
