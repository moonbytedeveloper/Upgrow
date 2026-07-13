using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Upgrow.Domain.Entities.WL
{
    [Table("WL_ActionLogs")]
    public class WL_ActionLogs
    {
        [Key]
        [Column(TypeName = "numeric")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal Id { get; set; }

        public int TenantId { get; set; }

        public string? Payload { get; set; }
        public byte[]? PreviousHash { get; set; }
        public byte[]? CurrentHash { get; set; }
        public byte[]? DigitalSignature { get; set; }

        public byte[]? RecordHash { get; set; }
    }
}