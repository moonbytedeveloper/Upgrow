using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTO.Verification.Common.Request
{
    public class CrimeCheckCompanyRequest
    {
        [Required(ErrorMessage = "CompanyName is required")]

        public string CompanyName { get; set; }
        public string? CompanyType { get; set; }
        public string? CompanyAddress { get; set; }
        public string? Directors { get; set; }
        public string? ReportMode { get; set; }
        public string? CinNumber { get; set; }
        public string? GstNumber { get; set; }
        public string? req_tag { get; set; }
        public string? TicketSize { get; set; }
        public string? CrimeWatch { get; set; }
    }
}
