using Microsoft.AspNetCore.Mvc;
using ProjectManager.Application.DTOs;
using ProjectManager.Application.Interfaces;

namespace ProjectManager.Main.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }
    
    [HttpGet("user")]
    public async Task<IActionResult> GetAll()
    {
        var users = await _userService.GetAllAsync();
        return Ok(users);
    }
    
    [HttpGet("user/{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var user = await _userService.GetByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        return Ok(user);
    }
    
    [HttpPost("user")]
    public async Task<IActionResult> Create([FromBody] UserDto userDto)
    {
        await _userService.CreateAsync(userDto);
        return NoContent();
    }
    
    [HttpPut("user")]
    public async Task<IActionResult> Update([FromForm] UserDto userDto)
    {
        if (userDto == null)
        {
            return BadRequest("User data is null.");
        }

        await _userService.UpdateAsync(userDto);
        return NoContent();
    }
    
    [HttpDelete("user/{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _userService.DeleteAsync(id);
        return NoContent();
    }
}