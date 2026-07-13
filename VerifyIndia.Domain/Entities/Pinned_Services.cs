using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Domain.Common;

namespace VerifyIndia.Domain.Entities
{
    public class Pinned_Services : BaseEntity
    {
        public string? ApiUUID { get; set; }
        public string? CustomerUUID { get; set; }
 
    }
}
