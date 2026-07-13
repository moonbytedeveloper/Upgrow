using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace VerifyIndia.Application.DTO.Verification.SprintVerify
{
    public class VoterValidateDto
    {
        public string refid { get; set; }
        [Required(ErrorMessage = "Required!")]
        public string id_number { get; set; }
    }
}
