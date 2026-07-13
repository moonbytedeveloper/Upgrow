using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.Verification.Common.Request
{
    public class CrimeCheckDownloadPdfReportRequest
    {
        [Required(ErrorMessage = "RequestId is required")]
        public string RequestId { get; set; }
    }
}
