using System;
using System.Collections.Generic;
using SmartWMS.Models.Enums;

namespace SmartWMS.Models;

public partial class Shelf
{

    public int ShelfId { get; set; }

    public string Lane { get; set; } = null!;

    public int Rack { get; set; }

    public LevelType Level { get; set; }

    public int MaxQuant { get; set; }

    public int CurrentQuant { get; set; }

    public int? ProductsProductId { get; set; }

    public virtual Product? ProductsProduct { get; set; }
}
 