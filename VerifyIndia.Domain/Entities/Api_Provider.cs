using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Domain.Common;

namespace VerifyIndia.Domain.Entities
{
    public class Api_Provider : BaseEntity
    {
 
        public string? ProviderName { get; set; }
        public string? Code { get; set; }
 
    }
}
