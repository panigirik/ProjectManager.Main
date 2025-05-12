using System.Security.Claims;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using ProjectManager.Application.Interfaces;
using ProjectManager.Application.RequestsDTOs;
using LoginRequest = ProjectManager.Application.RequestsDTOs.LoginRequest;

namespace ProjectManager.Main.Controllers;
//[Authorize]
public class AuthController: ControllerBase
{
    private readonly IAuthentificationService _authenticationService;

    public AuthController(IAuthentificationService authenticationService)
    {
        _authenticationService = authenticationService;
    }
    
    /// <summary>
    /// Выполняет вход пользователя, генерирует JWT токен.
    /// </summary>
    /// <param name="request">Запрос на вход с email и паролем.</param>
    /// <returns>Токен доступа и refresh токен.</returns>
    [HttpPost("login")]
    public async Task<ActionResult<LoginResult>> Login([FromBody] LoginRequest request)
    {
        var response = await _authenticationService.LoginAsync(request);
        return Ok(response);
    }
    
    /// <summary>
    /// Обновляет access токен с использованием refresh токена.
    /// </summary>
    /// <param name="request">Запрос с refresh токеном.</param>
    /// <returns>Новый access токен.</returns>
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromForm] RefreshRequest request)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        Guid.TryParse(userIdClaim, out var userId);
        var newAccessToken = await _authenticationService.RefreshTokenAsync(userId, request.RefreshToken);
        return Ok(new { AccessToken = newAccessToken });
    }
    
    /// <summary>
    /// Выход из системы, отзывается refresh токен.
    /// </summary>
    /// <returns>Сообщение об успешном выходе.</returns>
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        Guid.TryParse(userIdClaim, out var userId);
        await _authenticationService.LogoutAsync(userId);
        return NoContent();
    }
}