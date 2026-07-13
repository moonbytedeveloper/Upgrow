using System.ComponentModel.DataAnnotations;

namespace VerifyIndia.Application.DTO.Verification.SprintVerify
{
    public class CreditReportCheckExRequestDto
    {
        [Required(ErrorMessage = "Mobile is required")]
        public string Mobile { get; set; }
        [Required(ErrorMessage = "Pan is required")]
        public string Pan { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
    }

}
    