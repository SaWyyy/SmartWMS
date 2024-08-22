using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartWMS.Models;
using Task = System.Threading.Tasks.Task;

namespace SmartWMS.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly SmartwmsDbContext _dbContext;

    public UserController(UserManager<User> userManager, SignInManager<User> signInManager, SmartwmsDbContext dbContext)
    {
        this._userManager = userManager;
        this._signInManager = signInManager;
        this._dbContext = dbContext;
    }
    
    [HttpPost("register/manager")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> RegisterManager(Registration model)
    {
        var warehouse = await _dbContext.Warehouses.FirstOrDefaultAsync(x => x.WarehouseId == 1);
        if (warehouse is null)
        {
            return BadRequest("Warehouse not found");
        }
        
        var user = new User()
        {
            Email = model.Email,
            UserName = model.UserName,
            PasswordHash = model.Password,
            WarehousesWarehouseId = warehouse.WarehouseId
        };
        var result = await _userManager.CreateAsync(user, user.PasswordHash!);
        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, "Manager");
            return Ok("Registration succeeded");
        }
        
        return BadRequest("Registration failed");
    }
    
    [HttpPost("register/employee")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> RegisterEmployee(Registration model)
    {
        var warehouse = await _dbContext.Warehouses.FirstOrDefaultAsync(x => x.WarehouseId == 1);
        if (warehouse is null)
        {
            return BadRequest("Warehouse not found");
        }
        
        var user = new User()
        {
            Email = model.Email,
            UserName = model.UserName,
            PasswordHash = model.Password,
            WarehousesWarehouseId = warehouse.WarehouseId,
            ManagerId = model.ManagerId
        };
        var result = await _userManager.CreateAsync(user, user.PasswordHash!);
        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, "Employee");
            return Ok("Registration succeeded");
        }

        return BadRequest("Registration failed");
    }
}
