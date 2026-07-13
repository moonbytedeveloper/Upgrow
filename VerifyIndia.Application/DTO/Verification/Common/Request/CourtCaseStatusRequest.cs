using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.Verification.Common.Request
{
    public class CourtCaseStatusRequest : BaseDto
    {
        //public string? refid { get; set; }
        public string? name { get; set; }
        public string? father_name { get; set; }
        public string? address { get; set; }
        public string? state_name { get; set; }
        public string? year { get; set; }
        public string? source { get; set; }
        public string? case_type { get; set; }
        public string? search_type { get; set; }
        public string? filters { get; set; }
        public string? category { get; set; }
    }
}
