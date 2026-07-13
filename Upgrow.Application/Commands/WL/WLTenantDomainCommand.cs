using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Commands.Master;

namespace Upgrow.Application.Commands.WL
{
    public class WLTenantDomainCommand : IMasterCommand
    {
        public string? UUID { get; set; }

        [Required(ErrorMessage = "Domain  is required")]
        [RegularExpression(@"^(?=.{1,253}$)(?!-)([A-Za-z0-9](?:[A-Za-z0-9-]{0,61}[A-Za-z0-9])?)(\.(?!-)([A-Za-z0-9](?:[A-Za-z0-9-]{0,61}[A-Za-z0-9])?))*\.[A-Za-z]{2,63}$",
            ErrorMessage = "Domain must be a valid domain name (e.g., example.com).")]
        public string Domain { get; set; } = null!;

        [Required(ErrorMessage = "DomainType is required")]
        public string DomainType { get; set; } = null!;

        [Required(ErrorMessage = "Required!")]
        public int? TenantId { get; set; }
        public bool IsActive { get; set; }

        public bool IsPrimary { get; set; }
    }
}
