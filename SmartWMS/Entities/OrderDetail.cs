using SmartWMS.Models;

namespace SmartWMS.Entities;

public partial class OrderDetail
{
    public int OrderDetailId { get; set; }

    public int Quantity { get; set; }

    public int ProductsProductId { get; set; }

    public int OrderHeadersOrdersHeaderId { get; set; }

    public virtual OrderHeader OrderHeadersOrdersHeader { get; set; } = null!;

    public virtual Product ProductsProduct { get; set; } = null!;

    public virtual ICollection<Task>? TasksTask { get; set; } = null!;
}
