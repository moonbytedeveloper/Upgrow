using System.ComponentModel.DataAnnotations;

namespace VerifyIndia.Application.DTO.Verification.SprintVerify
{

    public class CompanyNameToCinRequestDto
    {
        public string refid { get; set; }
        [Required(ErrorMessage = "CompanyName is required")]
        public string CompanyName { get; set; }
    }
}
