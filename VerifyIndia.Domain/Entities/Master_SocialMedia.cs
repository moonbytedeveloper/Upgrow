using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Domain.Common;

namespace Upgrow.Domain.Entities
{
    public class Master_SocialMedia : BaseEntity
    {
        
        public string? PlatformName { get; set; }
        public string? ProfileURL { get; set; }
        public string? IconURL { get; set; }
        public decimal DisplayOrder { get; set; }
    }
}
