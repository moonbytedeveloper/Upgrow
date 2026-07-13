using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Domain.Common;

namespace Upgrow.Domain.Entities
{
    public class Knowledge_Hub : BaseEntity
    {
 
        public string? ImageURL { get; set; }
        public string? CategoryUUID { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? LongDescription { get; set; }
        public decimal? SequenceNo { get; set; }
    }
}
