﻿using System;
using System.Collections.Generic;

namespace SmartWMS.Models;

public partial class Task
{
    public int TaskId { get; set; }

    public DateTime? StartDate { get; set; }

    public string? FinishDate { get; set; }

    public int Priority { get; set; }

    public int OrderHeadersOrdersHeaderId { get; set; }

    public bool? Seen { get; set; }

    public virtual OrderHeader OrderHeadersOrdersHeader { get; set; } = null!;

    public virtual ICollection<ProductsHasTask> ProductsHasTasks { get; set; } = new List<ProductsHasTask>();

    public virtual ICollection<UsersHasTask> UsersHasTasks { get; set; } = new List<UsersHasTask>();
}
