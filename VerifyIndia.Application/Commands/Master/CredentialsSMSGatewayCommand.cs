using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.Commands.Master
{
    public class CredentialsSMSGatewayCommand : IMasterCommand
    {
        public string? UUID { get; set; }

        [Required(ErrorMessage = "API Key is required!")]
        [StringLength(50, ErrorMessage = "API Key must be between 10 and 50 characters")]
        [RegularExpression(@"^[a-zA-Z0-9_\-]+$", ErrorMessage = "API Key can only contain alphanumeric characters, underscores, and hyphens")]
        public string APIKey { get; set; } = null!;

        [Required(ErrorMessage = "Sender ID is required!")]
        [StringLength(50, ErrorMessage = "Sender ID must be between 10 and 50 characters")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Sender ID can only contain letters and numbers")]
        public string SenderId { get; set; } = null!;

        public bool IsActive { get; set; } = true;
    }
}
