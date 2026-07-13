using VerifyIndia.Domain.Common;

namespace VerifyIndia.Domain.Entities
{
    public class CustomerVideoKYC : BaseEntity
    {
      
        public string CustomerUUID { get; set; } = string.Empty;
        public string? VideoFileUrl { get; set; }
        public string KycText { get; set; } = string.Empty;
        public DateTimeOffset TimeStamp { get; set; }
        public bool IsKycDone { get; set; }
        public bool IsTimeout { get; set; }
        public string? OldRecordUUID { get; set; }       
    }
}
