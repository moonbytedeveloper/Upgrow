using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Domain.Entities;

namespace Upgrow.Domain.Entities
{
    public class Master_CMS : BaseEntity
    {       
        public string? PageTitle { get; set; }
        public string? UploadImage { get; set; }
        public string? Description { get; set; }     
        public string? Code { get; set; }
    }
}
