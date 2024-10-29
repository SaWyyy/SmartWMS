namespace SmartWMS.Entities;

public partial class Subcategory
{
    public int SubcategoryId { get; set; }

    public string SubcategoryName { get; set; } = null!;

    public bool IsDeleted { get; set; }

    public int CategoriesCategoryId { get; set; }

    public virtual Category CategoriesCategory { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
