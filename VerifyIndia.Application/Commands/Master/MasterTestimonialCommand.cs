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
    public class MasterTestimonialCommand :IMasterCommand
    {
        public string? UUID { get; set; }
        [Required(ErrorMessage = "Required!")]
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Only letters are allowed")]
        public string? CustomerName { get; set; }

        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Only letters are allowed")]
        public string? CompanyName { get; set; }

        [Required(ErrorMessage = "Required!")]
        public string? Comment { get; set; }

        public IFormFile? Image { get; set; }
        public string? FilePath { get; set; }
        [NotMapped]
        public string? ImageUrlwithdomain { get; set; }

        [Required(ErrorMessage = "Required!")]
        [Range(0, 5, ErrorMessage = "Star rating must be between 1 and 5")]
        public decimal? Star { get; set; }
        [Required(ErrorMessage = "Required!")]
        [Range(0, 99, ErrorMessage = "Sequence must be between 0 and 99")]
        public decimal? SequenceNo { get; set; }
        public bool IsActive { get; set; }
    }
}
