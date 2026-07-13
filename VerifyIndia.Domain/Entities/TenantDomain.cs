using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Domain.Common;

namespace Upgrow.Domain.Entities
{
    public class TenantDomain : BaseEntity
    {
 
        public int TenantId { get; set; }
        public string Domain { get; set; }
        public string DomainType { get; set; }
 
        public bool IsPrimary { get; set; }
        [NotMapped]
        public string? TenantName { get; set; }
        //[ForeignKey("TenantId")]
        //public Tenant Tenant { get; set; }
    }
}
