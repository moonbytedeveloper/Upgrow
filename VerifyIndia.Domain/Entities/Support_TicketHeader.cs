using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Domain.Entities
{
    public class Support_TicketHeader : BaseEntity
    {
        public string? TicketNumber { get; set; }
        public string? Subject { get; set; }
        public string? Otp { get; set; }
        public string? TicketCategoryUUID { get; set; }
        public string? UserType { get; set; }
        public string? UserUUID { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public string? AssigneeUUID { get; set; }

        [NotMapped]
        public string? RaisedByName { get; set; }

        [NotMapped]
        public string? RaisedByMobile { get; set; }

        [NotMapped]
        public string? RaisedByEmail { get; set; }

        [NotMapped]
        public string? Message { get; set; }
    }
}
