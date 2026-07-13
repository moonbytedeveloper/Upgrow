using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Domain.Common;

namespace VerifyIndia.Domain.Entities
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
