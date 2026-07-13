public class CategoryDto
{
    public string? UUID { get; set; }
    public string? Title { get; set; }
    public List<SubCategoryDto>? Subcategories { get; set; }
}