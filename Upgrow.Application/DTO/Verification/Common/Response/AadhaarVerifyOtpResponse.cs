using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.Verification.Common.Response
{
    public class AadhaarVerifyOtpResponse
    {
        public string? client_id { get; set; }
        public string? full_name { get; set; }
        public string? aadhaar_number { get; set; }
        public string? dob { get; set; }
        public string? gender { get; set; }

        public AadhaarAddress? address { get; set; }

        public bool? face_status { get; set; }
        public int? face_score { get; set; }

        public string? zip { get; set; }
        public string? profile_image { get; set; }

        public bool? has_image { get; set; }

        public string? email_hash { get; set; }
        public string? mobile_hash { get; set; }

        public string? raw_xml { get; set; }
        public string? zip_data { get; set; }

        public string? care_of { get; set; }
        public string? share_code { get; set; }

        public bool? mobile_verified { get; set; }

        public string? reference_id { get; set; }

        public string? aadhaar_pdf { get; set; }

        public string? status { get; set; }
        public string? uniqueness_id { get; set; }
    }

    public class AadhaarAddress
    {
        public string? country { get; set; }
        public string? dist { get; set; }
        public string? state { get; set; }
        public string? po { get; set; }
        public string? loc { get; set; }
        public string? vtc { get; set; }
        public string? subdist { get; set; }
        public string? street { get; set; }
        public string? house { get; set; }
        public string? landmark { get; set; }
    }
}
