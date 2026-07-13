using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Domain.Common;

namespace Upgrow.Domain.Entities
{
    public class Api_Provider : BaseEntity
    {
 
        public string? ProviderName { get; set; }
        public string? Code { get; set; }
 
    }
}
