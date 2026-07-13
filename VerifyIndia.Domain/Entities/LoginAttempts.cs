using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Domain.Entities
{
    [Table("LoginAttempts")]
    public class LoginAttempts  
    {
        [Key]
        [Column(TypeName = "numeric(18,0)")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal Id { get; set; }

        public string? Payload { get; set; }

        public byte[]? PreviousHash { get; set; }

        public byte[]? CurrentHash { get; set; }

        public byte[]? DigitalSignature { get; set; }

        public byte[]? RecordHash { get; set; }
    }
}
