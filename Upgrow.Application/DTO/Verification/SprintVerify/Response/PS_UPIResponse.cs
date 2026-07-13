using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.Verification.SprintVerify.Response
{
    public class PS_UPIResponse
    {
        public string? upi_id { get; set; }
        public bool? account_exists { get; set; }
        public string? full_name { get; set; }
        public string? remarks { get; set; }
        public string? ifsc_details { get; set; }
    }
}
