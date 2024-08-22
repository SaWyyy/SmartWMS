using System;
using System.Collections.Generic;

namespace SmartWMS.Models;

public partial class OrderDetail
{
    public int OrderDetailId { get; set; }

    public int Quantity { get; set; }

    public int ProductsProductId { get; set; }

    public int OrderHeadersOrdersHeaderId { get; set; }

    public virtual OrderHeader OrderHeadersOrdersHeader { get; set; } = null!;

    public virtual Product ProductsProduct { get; set; } = null!;
}
