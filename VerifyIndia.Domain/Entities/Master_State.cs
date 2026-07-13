using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Domain.Entities
{
    public class Master_State : BaseEntity
    {
        public string CountryUUID { get; set; }
        public string? Title { get; set; }
        public string? ShortTitle { get; set; }
    }
}
