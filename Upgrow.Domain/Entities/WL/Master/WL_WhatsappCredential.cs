using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Domain.Common;

namespace Upgrow.Domain.Entities.WL.Master
{
    public class WL_WhatsappCredential : TenantEntity
    {
        public string? ApiKey { get; set; }
        public string? SecretKey { get; set; }
        public string? MobileNumber { get; set; }
        public string? SenderName { get; set; }
    }
}
