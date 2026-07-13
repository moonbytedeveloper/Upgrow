using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Commands.Master;

namespace VerifyIndia.Application.Commands.WL.Master
{
    public class WLMasterPermissionGroupCommand : IMasterCommand
    {
        public string? UUID { get; set; }
        [Required(ErrorMessage = "Required!")]
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Only letters are allowed")]
        public string Title { get; set; }
        public bool IsActive { get; set; }
    }
}
