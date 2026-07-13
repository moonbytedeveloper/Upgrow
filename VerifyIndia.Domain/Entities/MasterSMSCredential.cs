using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Domain.Entities
{
    public class MasterSMSCredential
        : BaseEntity
    {
        public string Title { get; set; }
            = string.Empty;

        public string BaseUrl { get; set; }
            = string.Empty;

        public string AuthKey { get; set; }
            = string.Empty;
        public DateTimeOffset? CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
    }
}
