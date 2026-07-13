using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.WL
{
    public class WLTenantDto
    {
        public decimal id { get; set; }
        public string? UUID { get; set; }
        public string? Identifier { get; set; }
        public string? TenantName { get; set; }
        public bool IsPlatformOwner { get; set; }
        public bool IsActive { get; set; }
    }
}
