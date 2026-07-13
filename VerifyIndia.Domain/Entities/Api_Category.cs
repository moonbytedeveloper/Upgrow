using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Domain.Common;

namespace VerifyIndia.Domain.Entities
{
    public class Api_Category : BaseEntity
    {
 
        public string CategoryName {get; set;}
        public decimal SequenceNo { get; set; }
        public string Icon { get; set; }
 
    }
}
