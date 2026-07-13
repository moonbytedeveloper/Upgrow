using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.Commands.Master
{
    public class MasterIndustryCommand : IMasterCommand
    {
        public string? UUID { get; set; }

        [Required(ErrorMessage = "Required!")]
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Only letters are allowed")]
        public string? Title { get; set; }
        public string? Image { get; set; }

        public string? Icon { get; set; }
        [NotMapped]
        public string? ImageUrlwithdomain { get; set; }
        [NotMapped]
        public string? IconUrlwithdomain { get; set; }
        public string? Description { get; set; }

        public bool IsActive { get; set; }

        //Image Fields
        public IFormFile? IndustryImageFile { get; set; }
        public IFormFile? IndustryIconFile { get; set; }
    }
}
