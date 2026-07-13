using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Domain.Entities
{
    public class Support_TicketCategory : BaseEntity
    {
        public string? Title { get; set; }
        public string? DesignationUUID { get; set; }
        public string? UserType { get; set; }
    }
}
