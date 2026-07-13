using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.Commands.Master
{
    public class MasterFaqCommand : IMasterCommand
    {
        public string? UUID { get; set; }
        [Required(ErrorMessage = "Required!")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "Required!")]
        public string? FAQCategoryUUID { get; set; }
        [Required(ErrorMessage = "Required!")]
        public string? FAQSubCategoryUUID { get; set; }
        [Required(ErrorMessage = "Required!")]
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }
}
