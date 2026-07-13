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
    public class WLMasterCMSCommand : IMasterCommand
    {
        public string? UUID { get; set; }
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Only letters are allowed")]
        public string? PageTitle { get; set; }
        public string? UploadImage { get; set; }
        public string? TenantName { get; set; }
        public IFormFile? Image { get; set; }
        [NotMapped]
        public string? ImageUrlwithdomain { get; set; }

        [Required(ErrorMessage = "Required!")]
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        [Required(ErrorMessage = "Required!")]
        public int? TenantId { get; set; }
    }
}
