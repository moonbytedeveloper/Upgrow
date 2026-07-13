using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTO.Verification.Common.Response
{
    public class PincodeInfoResponse
    {
        public List<PincodeInfo>? data { get; set; }

    }
    public class PincodeInfo
    {
        public string? pincode { get; set; }
        public string? office_name { get; set; }
        public string? division_name { get; set; }
        public string? region_name { get; set; }
        public string? area_name { get; set; }
        public string? district_name { get; set; }
        public string? state_name { get; set; }
        public string? city_name { get; set; }
    }
}
