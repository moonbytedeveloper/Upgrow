using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.Verification.SprintVerify
{
    public class ReportFetchRequestDto
    {
        [Required(ErrorMessage = "transaction_id is required")]
        public string transaction_id { get; set; }
    }
}
