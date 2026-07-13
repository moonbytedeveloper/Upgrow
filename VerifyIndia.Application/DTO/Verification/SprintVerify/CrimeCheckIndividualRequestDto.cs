using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.Verification.SprintVerify
{
    public class CrimeCheckIndividualRequestDto
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        public string? FatherName { get; set; }
        public string? Address { get; set; }
        public string? Dob { get; set; }
        public string? PanNumber { get; set; }
        public string? req_tag { get; set; }
        public string? ReportMode { get; set; }
        public string? TicketSize { get; set; }
        public bool? Crimewatch { get; set; }
    }

    
}
