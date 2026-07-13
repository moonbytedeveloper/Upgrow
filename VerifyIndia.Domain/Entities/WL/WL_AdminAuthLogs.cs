using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Upgrow.Domain.Common;

namespace Upgrow.Domain.Entities.WL
{
    public class WL_AdminAuthLogs : ITenantEntity // Implementing this auto-handles TenantId via your AppDbContext
    {
        [Key]
        [Column(TypeName = "numeric(18,0)")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal Id { get; set; }

        public int TenantId { get; set; }

        public string? Payload { get; set; }

        public byte[]? PreviousHash { get; set; }

        public byte[]? CurrentHash { get; set; }

        public byte[]? DigitalSignature { get; set; }


    }
}