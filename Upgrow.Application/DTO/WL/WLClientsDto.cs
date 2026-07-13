using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.WL
{
    public class WLClientsDto
    {
        public string? UUID { get; set; }
        public string? Name { get; set; }
        public string? IconImage { get; set; }
        public int TenantId { get; set; }
        public bool IsActive { get; set; }
        public string? TenantName { get; set; }
    }
}
