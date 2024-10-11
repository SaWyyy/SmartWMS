namespace SmartWMS.Models.DTOs;

public class CategorySubcategoriesDto
{
    public int CategoryId { get; set; }
    public string CategoryName { get; set; }
    public virtual IEnumerable<SubcategoryDto> Subcategories { get; set; } = new List<SubcategoryDto>();
}