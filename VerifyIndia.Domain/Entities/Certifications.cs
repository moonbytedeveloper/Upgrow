using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Domain.Common;

namespace Upgrow.Domain.Entities
{
    public class Certifications : BaseEntity
    {
 
        public string? Name { get; set; }
        public string? FilePath { get; set; }
   
    }
}
