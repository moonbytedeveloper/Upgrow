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
    public class CertificationsCommand : IMasterCommand
    {
        public string? UUID { get; set; }
        [Required(ErrorMessage = "Required!")]
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Only letters are allowed")]
        public string? Name { get; set; }
        public string? FilePath { get; set; }
        public IFormFile? Image { get; set; }
        [NotMapped]
        public string? ImageUrlwithdomain { get; set; }
        public bool IsActive { get; set; }
    }
}
