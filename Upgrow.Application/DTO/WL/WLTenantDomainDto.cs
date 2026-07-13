using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.WL
{
    public class WLTenantDomainDto
    {
        public string? UUID { get; set; }
        public int TenantId { get; set; }
        public string? Domain { get; set; }
        public string? DomainType { get; set; }
        public bool IsPrimary { get; set; }
        public bool IsActive { get; set; }
        public string? TenantName { get; set; }
    }
}
