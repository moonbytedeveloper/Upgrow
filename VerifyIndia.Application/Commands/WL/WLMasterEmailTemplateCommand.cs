using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Commands.Master;

namespace VerifyIndia.Application.Commands.WL
{
    public class WLMasterEmailTemplateCommand : IMasterCommand
    {
        public string? UUID { get; set; }

        [Required(ErrorMessage = "Required!")]
        public string? EmailCredentialUUID { get; set; }

        [Required(ErrorMessage = "Required!")]
        public string EmailTemplateName { get; set; } = null!;

        [Required(ErrorMessage = "Required!")]
        public string EmailSubject { get; set; } = null!;

        [Required(ErrorMessage = "Required!")]
        public string? Description { get; set; }

        public bool IsActive { get; set; }
                
        public int? TenantId { get; set; }
    }
}
