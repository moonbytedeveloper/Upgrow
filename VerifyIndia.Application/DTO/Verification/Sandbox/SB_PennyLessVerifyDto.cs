using System.ComponentModel.DataAnnotations;

namespace Upgrow.Application.DTO.Verification.Sandbox
{
    public class SB_PennyLessVerifyDto
    {
        [Required(ErrorMessage = "Required!")]
        public string ifsc { get; set; } = string.Empty;

        [Required(ErrorMessage = "Required!")]
        public string account_number { get; set; } = string.Empty;
    }
}