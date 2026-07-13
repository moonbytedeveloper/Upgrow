using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Domain.Entities
{
    public class NotificationSound :BaseEntity
    {
        public string? SoundName { get; set; }
        public decimal? DisplayOrder { get; set; }
    }
}
