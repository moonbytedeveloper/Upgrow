using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Domain.Common;

namespace Upgrow.Domain.Entities
{
    public class Api_Category : BaseEntity
    {
 
        public string CategoryName {get; set;}
        public decimal SequenceNo { get; set; }
        public string Icon { get; set; }
 
    }
}
