using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Commands.Master;

namespace VerifyIndia.Application.Commands.AIX
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
