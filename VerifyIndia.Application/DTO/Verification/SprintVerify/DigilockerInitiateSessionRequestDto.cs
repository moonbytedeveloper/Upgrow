using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTO.Verification.SprintVerify
{
    public class DigilockerInitiateSessionRequestDto
    {
        public string refid { get; set; } // Unique Reference Id For each transaction
        [Required(ErrorMessage = "RedirectUrl is required")]
        public string redirecturl { get; set; } // Redirect URL for Digilocker
    }
    
}
