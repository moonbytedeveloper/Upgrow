using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Domain.Common;

namespace VerifyIndia.Domain.Entities
{
    public class AdminAuthLogs   
    {

        [Key]
        [Column(TypeName = "numeric(18,0)")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal Id { get; set; }


        public string? Payload { get; set; }

        public byte[]? PreviousHash { get; set; }

        public byte[]? CurrentHash { get; set; }

        public byte[]? DigitalSignature { get; set; }



    }
}
