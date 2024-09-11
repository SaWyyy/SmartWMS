using SmartWMS.Models;

namespace SmartWMS.Entities;

public partial class Product
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = null!;

    public string? ProductDescription { get; set; }

    public decimal Price { get; set; }

    public int WarehousesWarehouseId { get; set; }
    
    public int Quantity { get; set; }
    
    public string Barcode { get; set; }

    //public int ProductDetailsProductDetailId { get; set; }

    public int SubcategoriesSubcategoryId { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    //public virtual ProductDetail ProductDetailsProductDetail { get; set; } = null!;

    //public virtual ICollection<ProductsHasTask> ProductsHasTasks { get; set; } = new List<ProductsHasTask>();

    public virtual ICollection<Shelf> Shelves { get; set; } = new List<Shelf>();

    public virtual Subcategory SubcategoriesSubcategory { get; set; } = null!;

    public virtual Warehouse WarehousesWarehouse { get; set; } = null!;
}
