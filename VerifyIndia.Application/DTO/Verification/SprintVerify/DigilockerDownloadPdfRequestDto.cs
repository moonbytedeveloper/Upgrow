using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.Verification.SprintVerify
{
    public class DigilockerDownloadPdfRequestDto
    {
        public string refid { get; set; }
        [Required(ErrorMessage = "Uri is required")]
        public string Uri { get; set; }
    }

    
}
