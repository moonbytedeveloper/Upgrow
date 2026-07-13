using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Upgrow.Application.Commands.Master
{
    public class CredentialsWhatsappCommand : IMasterCommand, IEncryptable
    {
        public string? UUID { get; set; }
        [StringLength(50, ErrorMessage = "API Key must be between 10 and 50 characters")]
        [RegularExpression(@"^[a-zA-Z0-9_\-]+$", ErrorMessage = "API Key can only contain alphanumeric characters, underscores, and hyphens")]
        public string APIKey { get; set; } = null!;

        [Required(ErrorMessage = "Secret is required!")]
        [StringLength(50, ErrorMessage = "Secret must be between 10 and 50 characters")]
        //[RegularExpression(@"^[a-zA-Z0-9_\-]+$", ErrorMessage = "Secret can only contain alphanumeric characters, underscores, and hyphens")]
        public string AccessToken { get; set; } = null!;

        [Required(ErrorMessage = "Mobile Number is required!")]
        [StringLength(20, MinimumLength = 10, ErrorMessage = "Mobile Number must be exactly 10 digits")]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Mobile Number must be exactly 10 digits")]
        public string MobileNo { get; set; } = null!;
        [Required(ErrorMessage = "Sender Name is required!")]
        [StringLength(50, ErrorMessage = "Sender Name must be 50 characters")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Sender Name can only contain letters (A-Z, a-z)")]
        public string SenderName { get; set; }
        public bool IsActive { get; set; }

        public string? SensitiveValue
        {
            get => AccessToken;
            set => AccessToken = value;
        }
    }
}
