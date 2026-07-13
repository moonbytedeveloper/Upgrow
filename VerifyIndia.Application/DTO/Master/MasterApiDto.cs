using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTO.Master
{
    public class MasterApiDto
    {
        public string UUID { get; set; } = null!;
        public string ApiName { get; set; }
        public string Code { get; set; }
        public string ApiCategoryUUID { get; set; }
        public string ApiCategoryName { get; set; }
        public string ShortDescription { get; set; }
        public string? VerificationDocument { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsMultipleEndPoint { get; set; }
        public bool IsProviderSwitchable { get; set; }
        public bool IsActive { get; set; }
        public bool IsConsentBased { get; set; }
        public bool IsReminderRequired { get; set; }
        public string? ApiUUID { get; set; }
        public string? EndpointUrl { get; set; }
        public string? HttpMethod { get; set; }
    }
}
