using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTO.Verification.SprintVerify
{
    public class BulkUploadGstItemDto
    {
        [Required(ErrorMessage = "IdNumber is required")]
        public string id_number { get; set; }
        public string refid { get; set; }
    }
}
