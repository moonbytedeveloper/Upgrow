using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.Commands.Master
{
    public class MasterApiCommand : IMasterCommand
    {
        public string? UUID { get; set; }

        [Required(ErrorMessage = "API Name is required!")]
        [StringLength(150, ErrorMessage = "API Name cannot exceed 150 characters")]
        public string ApiName { get; set; }

        [StringLength(50, ErrorMessage = "Code cannot exceed 50 characters")]
        [RegularExpression(@"^[A-Z0-9_]*$", ErrorMessage = "Code must contain only uppercase letters, numbers, and underscores")]
        public string? Code { get; set; } // Read-only - auto-generated

        [Required(ErrorMessage = "API Category is required!")]
        public string ApiCategoryUUID { get; set; }

        [Required(ErrorMessage = "Description is required!")]
        [StringLength(350, ErrorMessage = "Description cannot exceed 350 characters")]
        public string ShortDescription { get; set; }

        [Required(ErrorMessage = "VerificationDocument is required!")]
        public string? VerificationDocument { get; set; }

        // DisplayOrder: 0 means auto-increment, 1-999 for manual override on edit
        [Required(ErrorMessage = "Sequence No is required!")]
        [Range(0, 999, ErrorMessage = "Sequence No must be between 0 and 999")]
        public int DisplayOrder { get; set; }
        public bool IsProviderSwitchable { get; set; }
        public bool IsActive { get; set; }
        public bool IsReminderRequired { get; set; }
        public bool IsConsentBased { get; set; }

        public bool IsMultipleEndPoint { get; set; }
    }
}
