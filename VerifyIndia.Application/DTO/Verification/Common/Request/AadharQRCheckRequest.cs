using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTO.Verification.Common.Request
{
    public class AadharQRCheckRequest
    {
        [Required(ErrorMessage = "Required!")]
        public IFormFile aadhaar_image { get; set; }
    }
}
