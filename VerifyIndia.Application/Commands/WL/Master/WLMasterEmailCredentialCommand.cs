using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.Commands.Master
{
    public class WLMasterEmailCredentialCommand : IMasterCommand
    {
        public string? UUID { get; set; }
       
        [Required(ErrorMessage = "Required!")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Invalid email address")]
        public string EmailAddress { get; set; } = null!;

        [Required(ErrorMessage = "Required!")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Required!")]
        public string HostServiceProvider { get; set; } = null!;

        [Required(ErrorMessage = "Required!")]
        public string? SMTP { get; set; }

        [Required(ErrorMessage = "Required!")]
        [Range(1, 65535, ErrorMessage = "Port must be between 1 and 65535")]
        public string? Port { get; set; }

        public bool IsActive { get; set; }
    }
}
