using System.ComponentModel.DataAnnotations;

namespace Upgrow.Application.DTO.Verification.Sandbox
{
    public class SB_IfscVerifyDto
    {
        [Required(ErrorMessage = "Required!")]
        public string ifsc { get; set; } = string.Empty;
    }
}