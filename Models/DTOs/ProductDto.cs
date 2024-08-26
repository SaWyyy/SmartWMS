namespace SmartWMS.Models;

public class ProductDto
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = null!;

    public string? ProductDescription { get; set; }

    public string Price { get; set; } = null!;

    public int WarehousesWarehouseId { get; set; }

    public int ProductDetailsProductDetailId { get; set; }

    public int SubcategoriesSubcategoryId { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ProductDetail ProductDetailsProductDetail { get; set; } = null!;

    public virtual ICollection<ProductsHasTask> ProductsHasTasks { get; set; } = new List<ProductsHasTask>();

    public virtual ICollection<ShelfDto> Shelves { get; set; } = new List<ShelfDto>();

    public virtual Subcategory SubcategoriesSubcategory { get; set; } = null!;

    public virtual Warehouse WarehousesWarehouse { get; set; } = null!;
}