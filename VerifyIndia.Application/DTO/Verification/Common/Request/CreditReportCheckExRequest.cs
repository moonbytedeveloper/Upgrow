using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTO.Verification.Common.Request
{
    public class CreditReportCheckExRequest
    {
        [Required(ErrorMessage = "Mobile is required")]
        public string Mobile { get; set; }
        [Required(ErrorMessage = "Pan is required")]
        public string Pan { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
    }
}
