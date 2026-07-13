using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.Commands.Master
{
    public class MasterEmployeeCommand : IMasterCommand, IValidatableObject
    {
        public string? UUID { get; set; }
        [Required(ErrorMessage = "Required!")]
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Only letters are allowed")]
        public string? FirstName { get; set; }
        [Required(ErrorMessage = "Required!")]
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Only letters are allowed")]
        public string? LastName { get; set; }
        [Required(ErrorMessage = "Required!")]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Mobile number must be exactly 10 digits")]
        public string? MobileNumber { get; set; }
        [Required(ErrorMessage = "Required!")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
         ErrorMessage = "Invalid email format")]
        public string? EmailId { get; set; }
        [Required(ErrorMessage = "Required!")]
        public string? GenderUUID { get; set; }
        [Required(ErrorMessage = "Required!")]
        public string? DepartmentUUID { get; set; }
        [Required(ErrorMessage = "Required!")]
        public string? RoleUUID { get; set; }
        [Required(ErrorMessage = "Required!")]
        public string? DesignationUUID { get; set; }
        [Required(ErrorMessage = "Required!")]
        public string? HonorificUUID { get; set; }
        [Required(ErrorMessage = "Required!")]
        public DateTime? BirthDate { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
         ErrorMessage = "Invalid email format")]
        public string? CompanyEmailID { get; set; }
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Mobile number must be exactly 10 digits")] 
        public string? CompanyMobileNumber { get; set; }
        [Required(ErrorMessage = "Required!")]
        public string? UserName { get; set; }
        public string? Password { get; set; }

        public bool IsLoginAllowed { get; set; }
        public bool IsActive { get; set; }
        public string? Profile_URL { get; set; }
        [NotMapped]
        public string? ProfileUrlwithdomain { get; set; }
        public IFormFile? Profile { get; set; }

        public string? EmployeeCode { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (BirthDate.HasValue && BirthDate.Value.Date > DateTime.Today)
            {
                yield return new ValidationResult(
                    "Birth date cannot be in the future",
                    new[] { nameof(BirthDate) }
                );
            }
        }
    }
}
