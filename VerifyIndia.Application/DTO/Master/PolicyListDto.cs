namespace Upgrow.Application.DTO.Master
{
    public class PolicyListDto
    {
        public string UUID { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string PolicyContent { get; set; } = string.Empty;
        public string Version { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public int SequenceNo { get; set; }

        public string ContentHash { get; set; } = string.Empty;
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
   
        public bool IsActive { get; set; }
    }
}
