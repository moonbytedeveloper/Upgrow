using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTO.Verification.Common.Request
{
    public class ReverseGeoLocationRequest : BaseDto
    {
        //public string refid { get; set; }
        [Required(ErrorMessage = "Required!")]
        public string latitude { get; set; }
        [Required(ErrorMessage = "Required!")]
        public string longitude { get; set; }
    }
}
