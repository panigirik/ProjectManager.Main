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

    // GET: api/users
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var users = await _userService.GetAllAsync();
        return Ok(users);
    }

    // GET: api/users/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var user = await _userService.GetByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        return Ok(user);
    }

    // POST: api/users
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] UserDto userDto)
    {
        if (userDto == null)
        {
            return BadRequest("User data is null.");
        }

        await _userService.CreateAsync(userDto);
        return CreatedAtAction(nameof(GetById), new { id = userDto.UserId }, userDto);
    }

    // PUT: api/users
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UserDto userDto)
    {
        if (userDto == null)
        {
            return BadRequest("User data is null.");
        }

        await _userService.UpdateAsync(userDto);
        return NoContent();
    }

    // DELETE: api/users/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _userService.DeleteAsync(id);
        return NoContent();
    }
}