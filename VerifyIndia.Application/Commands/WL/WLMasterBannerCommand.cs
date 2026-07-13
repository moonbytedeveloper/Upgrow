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
    public class WLMasterBannerCommand :IMasterCommand
    {
        public string? UUID { get; set; }

        [Required(ErrorMessage = "Required!")]
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Only letters are allowed")]
        public string MainTitle { get; set; } = null!;

        [Required(ErrorMessage = "Required!")]
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Only letters are allowed")]
        public string SubTitle { get; set; } = null!;
        public string? TenantName { get; set; }

        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Only letters are allowed")]
        public string? OptionalTitle { get; set; }

        [Required(ErrorMessage = "Required!")]
        public string ButtonText { get; set; } = null!;
        [Required(ErrorMessage = "Required!")]
        public string ButtonURL { get; set; } = null!;
        public string? BannerImage { get; set; }

        [NotMapped]
        public string? ImageUrlwithdomain { get; set; }
        public decimal? SequenceNo { get; set; }
        public bool IsActive { get; set; }
        //Image Fields
        public IFormFile? Image { get; set; }
        public List<SelectListItem> Tenants { get; set; } = new List<SelectListItem>();
      //  [Required(ErrorMessage = "Required!")]
        public int? TenantId { get; set; }
    }
}
