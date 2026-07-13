using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Domain.Common;

namespace Upgrow.Domain.Entities
{
    public class Knowledge_Hub_Category : BaseEntity
    {
 
        public string? Name { get; set; }
        public string? ShortDescription { get; set; }
        public string? IconImage { get; set; }
 
    }
}
