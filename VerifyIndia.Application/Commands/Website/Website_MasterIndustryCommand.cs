using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Commands.Master;
using Upgrow.Application.Common;

namespace Upgrow.Application.Commands.Website
{
    public class Website_MasterIndustryCommand : IMasterCommand 
    {
        public string? UUID { get; set; }

        [Required(ErrorMessage = "Required!")]
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Only letters are allowed")]
        [StringLength(50, ErrorMessage = "Title cannot exceed 50 characters")]
        public string Title { get; set; } = null!;
        [Required(ErrorMessage = "Required!")]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; }

        public string? ImageURL { get; set; }
        [NotMapped]
        public string? ImageUrlwithdomain { get; set; }
        public bool IsActive { get; set; }
        [Required(ErrorMessage = "Required!")]
        public int Sequence { get; set; }

        public IFormFile? WebIndustryImageFile { get; set; }

    }
}

 

 