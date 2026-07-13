using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Domain.Entities
{
    public class Inquiry_Partner : TenantEntity
    {
        public string UserName { get; set; }
        public string? MobileNo { get; set; }
        public string? EmailId { get; set; }
        public string? Message { get; set; }
        public string? Status { get; set; }
    }
}
