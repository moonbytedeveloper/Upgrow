using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.Commands.Master
{
    public class MasterEmailTemplateCommand : IMasterCommand
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
    }
}
