using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Commands.Master;

namespace VerifyIndia.Application.Commands.WL
{
    public class WLTestimonialCommand : IMasterCommand
    {
        public string? UUID { get; set; }
        [Required(ErrorMessage = "Required!")]
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Only letters are allowed")]
        public string? CustomerName { get; set; }

        public string? CompanyName { get; set; }
        [Required(ErrorMessage = "Required!")]
        public string? Comment { get; set; }

        [NotMapped]
        public string? ImageUrlwithdomain { get; set; }

        public IFormFile? Image { get; set; }
        public string? FilePath { get; set; }
        [Required(ErrorMessage = "Required!")]
        public decimal? Star { get; set; }
        [Required(ErrorMessage = "Required!")]
        public decimal? SequenceNo { get; set; }
        public bool IsActive { get; set; }

        //[Required(ErrorMessage = "Required!")]
        public int? TenantId { get; set; }
        public string? TenantName { get; set; }
        public List<SelectListItem> Tenants { get; set; } = new List<SelectListItem>();
    }
}
