using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Upgrow.Domain.Entities
{
    [Table("RegistrationApiLog")]
    public class RegistrationApiLog
    {
        [Key]
        [Column(TypeName = "numeric(18,0)")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal Id { get; set; }

        public string UUID { get; set; } = string.Empty;
        public string? CustomerUUID { get; set; }
        public string? MobileNo { get; set; }        
        public int? TenantId { get; set; }
        public string? ApiPath { get; set; }
        public string? HttpMethod { get; set; }
        public string? ApiRequest { get; set; }
        public string? ApiResponse { get; set; }
        public int? StatusCode { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public string? IpAddress { get; set; }

        public byte[]? RecordHash { get; set; }

    }
}
