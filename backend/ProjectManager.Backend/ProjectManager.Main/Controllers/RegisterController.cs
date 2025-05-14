using Microsoft.AspNetCore.Mvc;
using ProjectManager.Application.DTOs;
using ProjectManager.Application.Interfaces;

namespace ProjectManager.Main.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly IUserService _userService;

        public RegisterController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDto registerDto)
        {
            await _userService.CreateAsync(registerDto);
                return Ok(new { Message = "User registered successfully" });

            return BadRequest("Error occurred during registration.");
        }
    }
}