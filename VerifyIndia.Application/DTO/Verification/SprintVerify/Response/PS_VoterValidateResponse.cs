using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTO.Verification.SprintVerify.Response
{
    public class PS_VoterValidateResponse
    {
        public string? input_voter_id { get; set; }
        public string? epic_no { get; set; }
        public string? gender { get; set; }
        public string? state { get; set; }
        public string? name { get; set; }
        public string? relation_name { get; set; }
        public string? relation_type { get; set; }
        public string? house_no { get; set; }
        public string? dob { get; set; }
        public string? age { get; set; }
        public string? area { get; set; }
        public string? district { get; set; }

        [JsonPropertyName("additional_check")]
        public List<string> additional_check { get; set; }
        public string? multiple { get; set; }
        public string? last_update { get; set; }
        public string? assembly_constituency { get; set; }
        public string? assembly_constituency_number { get; set; }
        public string? polling_station { get; set; }
        public string? part_number { get; set; }
        public string? part_name { get; set; }
        public string? slno_inpart { get; set; }
        public string? ps_lat_long { get; set; }
        public string? rln_name_v1 { get; set; }
        public string? rln_name_v2 { get; set; }
        public string? rln_name_v3 { get; set; }
        public string? section_no { get; set; }
        public string? name_v1 { get; set; }
        public string? name_v2 { get; set; }
        public string? name_v3 { get; set; }
        public string? parliamentary_name { get; set; }
        public string? parliamentary_number { get; set; }
        public string? st_code { get; set; }
        public string? parliamentary_constituency { get; set; }
        public string? id { get; set; }
    }
}
