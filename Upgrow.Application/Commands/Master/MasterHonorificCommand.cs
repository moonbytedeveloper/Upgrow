using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.Commands.Master
{
    public class MasterHonorificCommand : IMasterCommand
    {
        public string? UUID { get; set; }

        [Required(ErrorMessage = "Required!")]
        [RegularExpression(@"^[A-Za-z\s.]+$", ErrorMessage = "Only letters and periods are allowed")]
        public string Title { get; set; } = null!;

        public bool IsActive { get; set; }
    }
}
