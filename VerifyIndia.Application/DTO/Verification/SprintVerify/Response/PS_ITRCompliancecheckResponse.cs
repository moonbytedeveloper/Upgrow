using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTO.Verification.SprintVerify.Response
{
    public class PS_ITRCompliancecheckResponse
    {
        public string? client_id { get; set; }
        public string? pan_number { get; set; }
        public bool? compliant { get; set; }
        public string? pan_allotment_date { get; set; }
        public string? masked_name { get; set; }

        public string? pan_aadhaar_linked { get; set; }
        public string? specified_person_under_206 { get; set; }
        public string? pan_status { get; set; }

        public bool? valid_pan { get; set; }
    }
}
