using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.Commands.Master
{
    public class MasterEmailCredentialCommand : IMasterCommand, IEncryptable
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

        public string? SensitiveValue
        {
            get => Password;
            set => Password = value;
        }
    }
}
