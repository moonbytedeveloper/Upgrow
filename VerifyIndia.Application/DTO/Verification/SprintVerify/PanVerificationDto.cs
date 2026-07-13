using System.ComponentModel.DataAnnotations;

namespace VerifyIndia.Application.DTO.Verification.SprintVerify
{
   
    public class PanVerifyRequestDto
    {
        public long refid { get; set; }
        [Required(ErrorMessage = "PanNumber is required")]
        public string pannumber { get; set; }
    }
}
