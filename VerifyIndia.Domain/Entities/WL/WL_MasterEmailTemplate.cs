using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Domain.Entities.WL
{
    public class WL_MasterEmailTemplate : TenantEntity
    {      
        public string EmailCredentialUUID { get; set; }
        public string EmailTemplateName { get; set; }
        public string EmailSubject { get; set; }        
        public string Description { get; set; }
        
        [NotMapped]
        public string? TenantName { get; set; }
    }
}
