using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.Commands.Master
{
    public class PasswordPolicyCommand : IMasterCommand, IValidatableObject
    {
        public string? UUID { get; set; }
        public bool IsActive { get; set; }

        [Required(ErrorMessage = "Required!")]
        [Range(1, 100, ErrorMessage = "Min Length must be between 1 and 100")]
        public int? MinLength { get; set; }

        public bool? UpperCase { get; set; } = false;

        public bool? LowerCase { get; set; } = false;

        public bool? AllowDigit { get; set; } = false;
        public bool? AllowSpecialChar { get; set; } = false;

        public string? SpecialCharacters { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
          
            if (AllowSpecialChar == true && string.IsNullOrWhiteSpace(SpecialCharacters))
            {
                yield return new ValidationResult(
                    "Special Characters are required when enabled",
                    new[] { nameof(SpecialCharacters) });
            }

            if (AllowSpecialChar != true)
            {
                SpecialCharacters = null;
            }
        }
    }
}
