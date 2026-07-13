using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Domain.Entities
{
    public class WL_Clients : TenantEntity
    {
        public string? Name { get; set; }
        public string? IconImage { get; set; }
        [NotMapped]
        public string? TenantName { get; set; }
    }
}
