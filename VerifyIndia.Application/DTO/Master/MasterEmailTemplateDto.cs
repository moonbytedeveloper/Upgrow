using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTO.Master
{
    public class MasterEmailTemplateDto
    {
        public decimal Id { get; set; }
        public string UUID { get; set; } = null!;
        public string EmailCredentialUUID { get; set; } = null!;
        public string EmailTemplateName { get; set; } = null!;
        public string EmailSubject { get; set; } = null!;
        public string Description { get; set; } = null!;
        public bool IsActive { get; set; }
    }
}
