using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Domain.Common;

namespace VerifyIndia.Domain.Entities
{
    public class Knowledge_Hub_Category : BaseEntity
    {
 
        public string? Name { get; set; }
        public string? ShortDescription { get; set; }
        public string? IconImage { get; set; }
 
    }
}
