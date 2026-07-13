using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.Commands.Master
{
    public class MasterFaqCategoryCommand : IMasterCommand
    {
        public string? UUID { get; set; }

        [Required(ErrorMessage = "Required!")]
        public string? Title { get; set; }
        [Required(ErrorMessage = "Required!")]
        public string? Icon { get; set; }
        [Required(ErrorMessage = "Required!")]
        public string? ShortDescription { get; set; }
        public bool IsActive { get; set; }
    }
}
