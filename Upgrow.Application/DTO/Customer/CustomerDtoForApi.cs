using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.Customer
{
    public class CustomerDtoForApi
    {
        public string? UUID { get; set; }
        public string? Mobile { get; set; }
        public string? FName { get; set; }
        public string? LName { get; set; }
        public string? IndustryUUID { get; set; }
        public string? StateUUID { get; set; }
        public string? StateName { get; set; }
        public string? CityUUID { get; set; }
        public string? CityName { get; set; }
        public string? ACType { get; set; }
        public string? EmailId { get; set; }
        public string? MaskedAadhaar { get; set; }
        public string? Avtar { get; set; }

    }
}
