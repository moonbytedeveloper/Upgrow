using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTO.Auth
{
    public class AdminPanelLoginDto
    {
        [Required(ErrorMessage = "Required!")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Required!")]
        public string Password { get; set; } = string.Empty;
    }
}
