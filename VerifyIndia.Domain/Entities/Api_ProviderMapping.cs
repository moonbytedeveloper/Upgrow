using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Domain.Common;

namespace VerifyIndia.Domain.Entities
{
    public class Api_ProviderMapping : BaseEntity
    {
 
        public string? ProviderUUID { get; set; }
        public decimal? Priority { get; set; }
        public string? ApiUUID { get; set; }        
 
    }
}
