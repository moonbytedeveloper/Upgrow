using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("VerificationRequestLogs")]
public class VerificationRequestLog
{
    [Key]
    [Column(TypeName = "numeric")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public decimal Id { get; set; }
    public string? Payload { get; set; }
    public byte[]? CurrentHash { get; set; }
    public byte[]? PreviousHash { get; set; }
    public byte[]? DigitalSignature { get; set; }

    public byte[]? RecordHash { get; set; }
}