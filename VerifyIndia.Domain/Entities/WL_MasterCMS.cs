using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Domain.Entities
{
    public class WL_MasterCMS : TenantEntity
    {
        public string? PageTitle { get; set; }
        public string? UploadImage { get; set; }
        public string? Description { get; set; }
        [NotMapped]
        public string? TenantName { get; set; }
    }
}
