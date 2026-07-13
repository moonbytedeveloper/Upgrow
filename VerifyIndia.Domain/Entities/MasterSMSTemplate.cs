using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Domain.Entities
{
    public class MasterSMSTemplate
        : BaseEntity
    {
        public string EventCode { get; set; }
            = string.Empty;

        public string SMSCredentialUUID { get; set; }
            = string.Empty;

        public string ProviderTemplateId { get; set; }
            = string.Empty;

        public DateTimeOffset? CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
    }
}
