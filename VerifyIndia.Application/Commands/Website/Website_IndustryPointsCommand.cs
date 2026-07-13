using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Commands.Master;
using VerifyIndia.Application.Common;

namespace VerifyIndia.Application.Commands.Website
{
    public class Website_IndustryPointsCommand : IMasterCommand 
    {
        public string? UUID { get; set; }

        [Required(ErrorMessage = "Required!")]
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Only letters are allowed")]
        [StringLength(50, ErrorMessage = "Title cannot exceed 50 characters")]
        public string Title { get; set; } = null!;
        [Required(ErrorMessage = "Required!")]
        public string IndustryUUID { get; set; } = null!;

        public bool IsActive { get; set; }

        [Required(ErrorMessage = "Required!")]
        public int Sequence { get; set; }

    }
}

 

 