using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTO.Verification.SprintVerify
{
    public class VpnProxyCheckRequestDto
    {
        public string refid { get; set; }

        [Required(ErrorMessage = "Required!")]
        public string ip_address { get; set; }
    }
}
