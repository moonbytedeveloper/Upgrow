using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Common;

namespace Upgrow.Application.Commands.Master
{
    public class MasterDesignationCommand : IMasterCommand ,IValidatableObject
    {
        public string? UUID { get; set; }

        [Required(ErrorMessage = "Required!")]
        [RegularExpression(@"^[A-Za-z0-9\s]+$", ErrorMessage = "Only letters, numbers and spaces are allowed")]
        public string Title { get; set; } = null!;

        [RegularExpression(@"^[A-Za-z0-9\s]+$", ErrorMessage = "Only letters, numbers and spaces are allowed")]
        public string? ShortTitle { get; set; }

        public bool IsActive { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var title = Title?.Trim() ?? string.Empty;
            var shortTitle = ShortTitle?.Trim() ?? string.Empty;

            if (title.Length is > 0 and < 2)
                yield return ValidationError.For(nameof(Title), "Title must be at least 2 characters long.");

            if (title.Length > 100)
                yield return ValidationError.For(nameof(Title), "Title cannot exceed 100 characters.");

            if (shortTitle.Length > 50)
                yield return ValidationError.For(nameof(ShortTitle), "Short Title cannot exceed 50 characters.");

            if (title.Length > 0 && shortTitle.Length > title.Length)
                yield return ValidationError.For(nameof(ShortTitle), "Short Title cannot be longer than Title.");
        }
    }
}