using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Domain.Common;

namespace Upgrow.Domain.Entities
{
    public class Api_ProviderComponentMapping : BaseEntity
    {
 
        public string? ApiUUID { get; set; }
        public string? ComponentUUID { get; set; }
 
        public int Sequence { get; set; }

    }
}
