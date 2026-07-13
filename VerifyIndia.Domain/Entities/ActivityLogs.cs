using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Domain.Entities
{    
    public class ActivityLogs
    {
        [Key]
        [Column(TypeName = "numeric")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal Id { get; set; }

        public byte[]? PreviousHash { get; set; }

        public byte[]? CurrentHash { get; set; }

        public byte[]? DigitalSignature { get; set; }

        public string? Payload { get; set; }
    }
}
