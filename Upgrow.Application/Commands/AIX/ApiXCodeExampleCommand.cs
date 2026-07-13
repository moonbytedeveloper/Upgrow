using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Commands.Master;

namespace Upgrow.Application.Commands.AIX
{
    public class ApiXCodeExampleCommand : IMasterCommand
    {
        public string? UUID { get; set; }
        [Required(ErrorMessage = "Required!")]
        public string? StatusCodeUUID { get; set; }
        [Required(ErrorMessage = "Required!")]
        public string? ErrorTitle { get; set; }
        public string? Message { get; set; }
        public bool IsActive { get; set; }
    }
}
