using System;
using System.Collections.Generic;

namespace SmartWMS.Models;

public partial class ProductDetail
{
    public int ProductDetailId { get; set; }

    public int Quantity { get; set; }

    public string Barcode { get; set; } = null!;

    public virtual Product ProductsProduct { get; set; } = null!;
}
