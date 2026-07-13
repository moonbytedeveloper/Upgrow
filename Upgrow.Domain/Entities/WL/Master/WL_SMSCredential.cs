using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Domain.Entities.WL.Master
{
    public class WL_SMSCredential : TenantEntity
    {
        public string? ApiKey { get; set; }
        public string? SenderId { get; set; }
    }
}
