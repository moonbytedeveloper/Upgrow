using System.ComponentModel.DataAnnotations;
using Upgrow.Application.DTO.Master;

namespace UpgrowAdminPanel.Models.Master
{
    public class ChangePasswordVM
    {
        public string UUID { get; set; }

        [Required(ErrorMessage = "Required!")]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "Required!")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Required!")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Required!")]
        public string Reason { get; set; }

        public PasswordPolicyDto? PasswordPolicy { get; set; }

        public string? PasswordPolicyDescription { get; set; }
    }
}
