using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Domain.Entities.Inquiry
{
    public class Inquiry_Agent : BaseEntity
    {
       
        public string? FName { get; set; }
        public string? MName { get; set; }
        public string? LName { get; set; }
        public string? Email { get; set; }
        public bool SalesExperience { get; set; }
        public string? PhoneNo { get; set; }
        public string? SalesExperienceDescription { get; set; }
        public string? StateUUID { get; set; }
        public string? CityUUID { get; set; }
        public string? HasExistingClients { get; set; }
        public string? Message { get; set; }
        public string? Remark { get; set; }
        public string? ActionTakenBy { get; set; }

        public bool IsConvertedToAgent { get; set; }

        public bool IsStatusClosed { get; set; }

    }
}
