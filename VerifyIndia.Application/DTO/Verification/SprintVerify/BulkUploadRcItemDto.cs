using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.Verification.SprintVerify
{
    public class BulkUploadRcItemDto
    {
        [Required(ErrorMessage = "IdNumber is required")]
        public string id_number { get; set; }
        public string refid { get; set; }
    }
}
