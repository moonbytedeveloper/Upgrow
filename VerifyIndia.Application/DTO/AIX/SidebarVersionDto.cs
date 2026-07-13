namespace Upgrow.Application.DTO.AIX
{
    public class SidebarVersionDto
    {
        public string UUID { get; set; }
        public string Title { get; set; }
        public string ApiPath { get; set; }
        public string ApiMethod { get; set; }

        public bool IsRequestBodyRequired { get; set; }
        public string Description { get; set; }
        public string VersionNo { get; set; }
        public decimal SequenceNo { get; set; }
    }
}
