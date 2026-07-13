using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Common;

namespace VerifyIndia.Application.Commands.Master
{
    public class ApiXCategoryCommand : IMasterCommand, IValidatableObject
    {
        public string? UUID { get; set; }

        [Required(ErrorMessage = "Required!")]
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Only letters are allowed")]
        public string Title { get; set; } = null!;
        [Required(ErrorMessage = "Required!")]
        public string? Description { get; set; }

        
        public int SequenceNo { get; set; }
        [Required(ErrorMessage = "Required!")]
        public string Icon { get; set; }
        [Required(ErrorMessage = "Required!")]
        public string CategoryUUID { get; set; }
        public bool IsActive { get; set; }

        public List<SelectListItem> ApiCategoryList { get; set; } = new();
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var title = Title?.Trim() ?? string.Empty;
            var description = Description?.Trim() ?? string.Empty;

            if (title.Length is > 0 and < 2)
                yield return ValidationError.For(nameof(Title), "Title must be at least 2 characters long.");

            if (title.Length > 50)
                yield return ValidationError.For(nameof(Title), "Title cannot exceed 50 characters.");

            if (description.Length > 500)
                yield return ValidationError.For(nameof(description), "Description cannot exceed 500 characters.");
 
        }
    }
}

 

 