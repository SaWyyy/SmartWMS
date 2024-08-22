using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartWMS.Models;
using SmartWMS.Repositories;
using Task = System.Threading.Tasks.Task;

namespace SmartWMS.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    public UserController(IUserRepository userRepository)
    {
        this._userRepository = userRepository;
    }
    
    [HttpPost("register/manager")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> RegisterManager(Registration model)
    {
        var result = await _userRepository.RegisterManager(model);
        
        if (result == IdentityResult.Failed())
            return BadRequest("Registration failed");
        
        return Ok("Registration successful");
    }
    
    [HttpPost("register/employee")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> RegisterEmployee(Registration model)
    {
        var result = await _userRepository.RegisterEmployee(model);

        if (result == IdentityResult.Failed())
            return BadRequest("Registration failed");
        
        return Ok("Registration successful");
    }
}
