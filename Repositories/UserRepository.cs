using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using SmartWMS.Entities;
using SmartWMS.Models;
using SmartWMS.Models.DTOs;
using SmartWMS.Repositories.Interfaces;

namespace SmartWMS.Repositories;

public class UserRepository : IUserRepository
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly SmartwmsDbContext _dbContext;
    private readonly IMapper _mapper;

    public UserRepository(
        UserManager<User> userManager, 
        SignInManager<User> signInManager, 
        RoleManager<IdentityRole> roleManager, 
        IMapper mapper, 
        SmartwmsDbContext dbContext)
    {
        this._userManager = userManager;
        this._signInManager = signInManager;
        this._dbContext = dbContext;
        this._roleManager = roleManager;
        this._mapper = mapper;
    }
    
    public async Task<IdentityResult> RegisterManager(Registration model)
    {
        var warehouse = await _dbContext.Warehouses.FirstOrDefaultAsync(x => x.WarehouseId == 1);
        if (warehouse is null)
        {
            return IdentityResult.Failed(new IdentityError{Description = "Warehouse is mandatory!"});
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
            return result;
        }
        
        return IdentityResult.Failed(new IdentityError{ Description = "Registration failed"} );
    }
    
    public async Task<IdentityResult> RegisterEmployee(Registration model)
    {
        var warehouse = await _dbContext.Warehouses.FirstOrDefaultAsync(x => x.WarehouseId == 1);
        if (warehouse is null)
        {
            return IdentityResult.Failed(new IdentityError{Description = "Warehouse is mandatory!"});
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
            return result;
        }

        return IdentityResult.Failed(new IdentityError{Description = "Registration failed"});
    }

    public async Task<IEnumerable<UserDto>> GetUsers(string roleName)
    {
        var managerRole = await _roleManager.FindByNameAsync(roleName);

        if (managerRole is null)
            throw new SmartWMSExceptionHandler("No users with provided role name in the system");

        var users = await _dbContext.Users
            .Where(user => _dbContext.UserRoles
                .Any(ur => ur.UserId == user.Id && ur.RoleId == managerRole.Id))
            .ToListAsync();
        
        return _mapper.Map<List<UserDto>>(users);
    }
}