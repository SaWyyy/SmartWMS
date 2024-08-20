using System;
using System.Collections.Generic;

namespace SmartWMS.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Email { get; set; } = null!;

    public int? ManagerId { get; set; }

    public int WarehousesWarehouseId { get; set; }

    public virtual ICollection<User> InverseManager { get; set; } = new List<User>();

    public virtual User? Manager { get; set; }

    public virtual Warehouse WarehousesWarehouse { get; set; } = null!;

    public virtual ICollection<UsersHasTask> UsersHasTasks { get; set; } = new List<UsersHasTask>();
}
