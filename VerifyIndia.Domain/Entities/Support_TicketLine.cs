using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Domain.Entities
{
    public class Support_TicketLine : BaseEntity
    {
        public string? HeaderUUID { get; set; }
        public string? UserUUID { get; set; }
        public string? Message { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }

    }
}
