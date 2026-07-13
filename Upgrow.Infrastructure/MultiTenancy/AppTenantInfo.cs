using Finbuckle.MultiTenant.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Infrastructure.MultiTenancy
{
    public class AppTenantInfo : ITenantInfo
    {
        public string Id { get; set; }
        public string Identifier { get; set; }
        public string Name { get; set; }
    }
}
