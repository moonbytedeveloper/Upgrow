using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Domain.Entities
{
    public class WL_MasterSocialMedia : TenantEntity
    {
        public string? PlatformName { get; set; }
        public string? ProfileURL { get; set; }
        public string? IconURL { get; set; }
        public decimal DisplayOrder { get; set; }
        [NotMapped]
        public string? TenantName { get; set; }
    }
}
