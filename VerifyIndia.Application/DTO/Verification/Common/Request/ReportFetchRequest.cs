using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTO.Verification.Common.Request
{
    public class ReportFetchRequest
    {
        [Required(ErrorMessage = "transaction_id is required")]
        public string transaction_id { get; set; }
    }
}
