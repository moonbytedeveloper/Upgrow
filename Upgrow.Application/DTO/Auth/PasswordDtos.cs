using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.DTO.Master;

namespace Upgrow.Application.DTO.Auth
{
    public class ForgotPasswordDto
    {
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Invalid email address")]
        public string? Email { get; set; }
        public string?BaseURL { get; set; }

    }

    public class ResetPasswordDto
    {
        public string Token { get; set; }

        [Required(ErrorMessage = "Required!")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Required!")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        public PasswordPolicyDto? PasswordPolicy { get; set; }

        public string? PasswordPolicyDescription { get; set; }
    }
    public class PasswordResetToken
    {
        public int Id { get; set; }
        public string EmployeeUUID { get; set; }
        public string Token { get; set; }
        public DateTime Expiry { get; set; }
        public bool IsUsed { get; set; }

    }
}
