using Upgrow.Domain.Common;

namespace Upgrow.Domain.Entities
{
    public class Master_Policy : BaseEntity
    {
 
        public string? Title { get; set; } = string.Empty;
        public string? ContentHash { get; set; } = string.Empty;
        public string? PolicyContent { get; set; } = string.Empty;
        public string? Version { get; set; } = string.Empty;
        public string? Code { get; set; } = string.Empty;
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int SequenceNo { get; set; }
   
    }
}
