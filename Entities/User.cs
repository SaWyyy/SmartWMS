using Microsoft.AspNetCore.Identity;
using SmartWMS.Models;

namespace SmartWMS.Entities;

public partial class User : IdentityUser
{
    public string? ManagerId { get; set; }

    public int WarehousesWarehouseId { get; set; }

    public virtual ICollection<User> InverseManager { get; set; } = new List<User>();

    public virtual User? Manager { get; set; }

    public virtual Warehouse WarehousesWarehouse { get; set; } = null!;

    public virtual ICollection<UsersHasTask> UsersHasTasks { get; set; } = new List<UsersHasTask>();
}
