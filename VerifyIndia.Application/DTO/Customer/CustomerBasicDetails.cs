using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTO.Customer
{
    public class CustomerBasicDetails
    {
        public string? EmailId { set; get; }
        public string? StateUUID { set; get; }
        public string? CityUUID { set; get; }
        public string? IndustryUUID { set; get; }
        public string? ACType { set; get; }
    }
}
