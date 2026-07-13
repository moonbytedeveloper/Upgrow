using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Domain.Common;

namespace Upgrow.Domain.Entities
{
    public class WL_MasterEmailCredential : TenantEntity
    {
        
        public string? EmailAddress { get; set; }
        public string? Password { get; set; }
        public string? HostServiceProvider { get; set; }
        public string? SMTP { get; set; }
        public string? Port { get; set; }

    }
}
