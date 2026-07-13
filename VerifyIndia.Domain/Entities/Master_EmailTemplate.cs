using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Domain.Entities
{
    public class Master_EmailTemplate : BaseEntity
    {      
        public string? EmailCredentialUUID { get; set; }
        public string? EmailTemplateName { get; set; }
        public string? EmailSubject { get; set; }
        public string? Description { get; set; }

        public string? TemplateCode { get; set; }
    }
}
