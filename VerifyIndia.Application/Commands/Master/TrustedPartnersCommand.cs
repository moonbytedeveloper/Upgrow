using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.Commands.Master
{
    public class TrustedPartnersCommand : IMasterCommand
    {
        public string? UUID { get; set; }

        [Required(ErrorMessage = "Required!")]
        [RegularExpression(@"^[A-Za-z0-9\s\-_.,!@#$%^&*()'""/:;?]+$", ErrorMessage = "Special characters and numbers are allowed")]
        public string? Name { get; set; }

        public string? IconImage { get; set; }

        [NotMapped]
        public string? ImageUrlwithdomain { get; set; }
        public IFormFile? Image { get; set; }

        public bool IsActive { get; set; }
    }
}
