using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Domain.Common;

namespace VerifyIndia.Domain.Entities
{
    public class Tenant_Domain : BaseEntity
    {
 
        public decimal TenantId { get; set; }
        public string Domain { get; set; }
        public string DomainType { get; set; }
 
        public bool IsPrimary { get; set; }
        
    }
}
