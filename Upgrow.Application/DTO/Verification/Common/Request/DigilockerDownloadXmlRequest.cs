using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.Verification.Common.Request
{
    public class DigilockerDownloadXmlRequest : BaseDto
    {
        //public string refid { get; set; }
        [Required(ErrorMessage = "Uri is required")]
        public string Uri { get; set; }
    }
}
