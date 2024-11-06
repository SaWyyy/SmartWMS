using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
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
    private readonly IHttpContextAccessor _accessor;

    public UserRepository(
        UserManager<User> userManager, 
        SignInManager<User> signInManager, 
        RoleManager<IdentityRole> roleManager, 
        SmartwmsDbContext dbContext,
        IHttpContextAccessor accessor
        )
    {
        this._userManager = userManager;
        this._signInManager = signInManager;
        this._dbContext = dbContext;
        this._roleManager = roleManager;
        this._accessor = accessor;
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

        var manager = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == model.ManagerId);
        if(manager is null)
            return IdentityResult.Failed(new IdentityError{Description = "Manager does not exist"});
        
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

    public async Task<IEnumerable<UserDto>> GetUsers(string? roleName)
    {
        if (roleName is null)
        {
            var result = await _dbContext.Users
                .Where(user => _dbContext.UserRoles
                    .Any(ur => ur.UserId == user.Id))
                .Select(user => new {
                    User = user,
                    RoleId = _dbContext.UserRoles
                        .Where(ur => ur.UserId == user.Id)
                        .Select(ur => ur.RoleId)
                        .FirstOrDefault()
                })
                .ToListAsync();


            var userDtos = new List<UserDto>();
            foreach (var user in result)
            {
                var role = await _roleManager.FindByIdAsync(user.RoleId!);

                userDtos.Add(new UserDto
                {
                    Id = user.User.Id,
                    Email = user.User.Email!,
                    UserName = user.User.UserName!,
                    Role = role?.Name!,
                    ManagerId = user.User.ManagerId!
                });
            }

            return userDtos;
        }
        
        var managerRole = await _roleManager.FindByNameAsync(roleName);

        if (managerRole is null)
            throw new SmartWMSExceptionHandler("No users with provided role name in the system");

        var users = await _dbContext.Users
            .Where(user => _dbContext.UserRoles
                .Any(ur => ur.UserId == user.Id && ur.RoleId == managerRole.Id))
            .ToListAsync();

        var usersDto = new List<UserDto>();
        
        foreach (var user in users)
        {
            var singleUser = new UserDto
            {
                Id = user.Id,
                Email = user.Email!,
                UserName = user.UserName!,
                ManagerId = user.ManagerId!,
                Role = roleName
            };
            
            usersDto.Add(singleUser);
        }

        return usersDto;
    }

    public async Task<UserDto> GetUser()
    {
        var user = _accessor.HttpContext?.User;

        if (user is null)
            throw new SmartWMSExceptionHandler("Logged user not found");
        
        var userId = user!.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        var email = user!.FindFirst(ClaimTypes.Email)!.Value;
        var userName = user!.FindFirst(ClaimTypes.Name)!.Value;
        var role = user!.FindFirst(ClaimTypes.Role)!.Value;
        var tempUser = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId);
        var managerId = tempUser!.ManagerId;

        var userDto = new UserDto
        {
            Id = userId,
            Email = email,
            UserName = userName,
            ManagerId = managerId,
            Role = role
        };

        return userDto;
    }

    public async Task<IdentityResult> DeleteUser(string id)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == id);
        
        if (user is null)
            return IdentityResult.Failed( new IdentityError { Description = "User not found"} );

        var result = await _userManager.DeleteAsync(user);
        if (result.Succeeded)
            return result;
        
        return IdentityResult.Failed( new IdentityError{ Description = "Deleting failed" } );
    }
}