using System;
using System.Collections.Generic;

namespace SmartWMS.Models;

public partial class Warehouse
{
    public int WarehouseId { get; set; }

    public string Address { get; set; } = null!;

    public virtual ICollection<Alert> Alerts { get; set; } = new List<Alert>();

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    public virtual ICollection<Report> Reports { get; set; } = new List<Report>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
