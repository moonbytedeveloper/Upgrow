using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.Verification.SprintVerify.Response
{
    public class PS_PincodeInfoResponse
    {
        public List<PincodeData>? data { get; set; }

    }
    public class PincodeData
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
