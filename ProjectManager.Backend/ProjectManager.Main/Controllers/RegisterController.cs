using Microsoft.AspNetCore.Mvc;
using ProjectManager.Application.DTOs;
using ProjectManager.Domain.Entities;
using ProjectManager.Application.Interfaces;
using System.Threading.Tasks;

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
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (await _userService.UserExistsAsync(registerDto))
                return BadRequest("Username is already taken.");


            var registrationSuccess = await _userService.RegisterUserAsync(registerDto);

            if (registrationSuccess)
                return Ok(new { Message = "User registered successfully" });

            return BadRequest("Error occurred during registration.");
        }
    }
}