using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Domain.Entities.Inquiry
{
    public class Inquiry_Distributor : BaseEntity
    {
        public string? FullName { get; set; }
        public string? EmailId { get; set; }
        public string? PhoneNo { get; set; }
        public string? CompanyName { get; set; }
        public string? BusinessType { get; set; }
        public string? StateUUID { get; set; }
        public string? CityUUID { get; set; }
        public string? ExpectedRetailers { get; set; }
        public string? Message { get; set; }
        public string? Remark { get; set; }
        public string? ActionTakenBy { get; set; }

    }
}
