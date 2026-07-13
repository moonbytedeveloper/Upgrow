using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Domain.Common;

namespace Upgrow.Domain.Entities
{
    public class ApiXCategory : BaseEntity
    {
        public string Title { get; set; }
        public string CategoryUUID { get; set; }
        public string Description {get; set;}
        public string Icon{get; set;}
        public int SequenceNo{get; set;}   
    }
}
