namespace Upgrow.Application.DTO.Website
{
    public class Website_IndustryCasesDto
    {
        public string UUID { get; set; }

        public string Icon { get; set; }
        public string IndustryUUID { get; set; }

        public string Title { get; set; }

        public int Sequence { get; set; }
        public bool IsActive { get; set; }
    }
}
