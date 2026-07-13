using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Domain.Common;

namespace Upgrow.Domain.Entities
{
    public class Tenant: BaseEntity
    {
 
        public string Identifier { get; set; }
        public string TenantName { get; set; }
        public bool IsPlatformOwner { get; set; }
 
        public string? SecretHash { get; set; }
        public string? Mobile { get; set; }
        public string? Email { get; set; }


    }
}
