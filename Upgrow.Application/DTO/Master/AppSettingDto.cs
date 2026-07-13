namespace Upgrow.Application.DTO.Master
{
    public class AppSettingDto  
    {
        public string? UUID { get; set; }
        public string Key { get; set; } = null!;
        public string? Value { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }   
    }
}
