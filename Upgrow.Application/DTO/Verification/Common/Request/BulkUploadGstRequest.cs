using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.Verification.Common.Request
{
    public class BulkUploadGstRequest : BaseDto
    {
        [Required(ErrorMessage = "IdNumber is required")]
        public string id_number { get; set; }
        //public string refid { get; set; }
    }
}
