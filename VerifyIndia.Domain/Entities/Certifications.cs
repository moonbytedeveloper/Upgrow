using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Domain.Common;

namespace VerifyIndia.Domain.Entities
{
    public class Certifications : BaseEntity
    {
 
        public string? Name { get; set; }
        public string? FilePath { get; set; }
   
    }
}
