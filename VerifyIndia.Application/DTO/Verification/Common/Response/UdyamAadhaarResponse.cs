using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTO.Verification.Common.Response
{
    public class UdyamAadhaarResponse
    {
        public string? client_id { get; set; }
        public string? uan { get; set; }
        public string? certificate_url { get; set; }

        public MainDetails? main_details { get; set; }

        public List<LocationOfPlantDetail>? location_of_plant_details { get; set; }

        public List<NicCode>? nic_code { get; set; }
    }
    public class MainDetails
    {
        public List<EnterpriseType>? enterprise_type_list { get; set; }

        public string? name_of_enterprise { get; set; }
        public string? major_activity { get; set; }
        public string? social_category { get; set; }
        public string? date_of_commencement { get; set; }
        public string? dic_name { get; set; }
        public string? state { get; set; }
        public string? applied_date { get; set; }
        public string? flat { get; set; }
        public string? name_of_building { get; set; }
        public string? road { get; set; }
        public string? village { get; set; }
        public string? block { get; set; }
        public string? city { get; set; }
        public string? pin { get; set; }
        public string? mobile_number { get; set; }
        public string? email { get; set; }
        public string? organization_type { get; set; }
        public string? gender { get; set; }
        public string? date_of_incorporation { get; set; }
        public string? msme_dfo { get; set; }
        public string? registration_date { get; set; }
    }
    public class EnterpriseType
    {
        public string? classification_year { get; set; }
        public string? enterprise_type { get; set; }
        public string? classification_date { get; set; }
    }

    public class LocationOfPlantDetail
    {
        public string? unit_name { get; set; }
        public string? line_1 { get; set; }
        public string? building { get; set; }
        public string? village { get; set; }
        public string? street { get; set; }
        public string? road { get; set; }
        public string? city { get; set; }
        public string? pin { get; set; }
        public string? state { get; set; }
        public string? district { get; set; }
    }
    public class NicCode
    {
        public string? nic_2_digit { get; set; }
        public string? nic_4_digit { get; set; }
        public string? nic_5_digit { get; set; }
        public string? activity_type { get; set; }
        public string? added_on { get; set; }
    }
}
