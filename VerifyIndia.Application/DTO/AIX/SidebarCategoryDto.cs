
namespace Upgrow.Application.DTO.AIX
{
    public class SidebarCategoryDto
    {
        public string UUID { get; set; }
        public string CategoryName { get; set; }
        public string Icon { get; set; }
        public decimal SequenceNo { get; set; }
        public List<SidebarXCategoryDto> XCategories { get; set; } = new();
    }

}
