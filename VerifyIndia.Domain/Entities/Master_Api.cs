using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Domain.Common;

namespace VerifyIndia.Domain.Entities
{
    public class Master_Api : BaseEntity
    {
 
        public string ApiName { get; set; }
        public string Code { get; set; } 
        public string ApiCategoryUUID { get; set; }
        public string ShortDescription { get; set; }
        public string? VerificationDocument { get; set; }
        public int DisplayOrder { get; set; }
        public bool? IsConsentBased { get; set; }
        public bool? IsReminderRequired { get; set; }
        public bool? IsProviderSwitchable { get; set; }
 

        public bool IsMultipleEndPoint { get; set; }
    }
}
