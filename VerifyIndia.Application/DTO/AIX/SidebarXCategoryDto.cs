 
namespace Upgrow.Application.DTO.AIX
{
    public class SidebarXCategoryDto
    {
        public string UUID { get; set; }
        public string Title { get; set; }
        public string Icon { get; set; }
        public decimal SequenceNo { get; set; }
        public string Description { get; set; }
        public List<SidebarVersionDto> Versions { get; set; } = new();
    }

}
