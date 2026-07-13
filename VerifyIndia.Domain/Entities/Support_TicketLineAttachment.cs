using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Domain.Entities
{
    public class Support_TicketLineAttachment : BaseEntity
    {
        public string LineUUID { get; set; }
        public string? URL { get; set; }

    }
}
