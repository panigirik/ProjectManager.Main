using Microsoft.AspNetCore.Mvc;

namespace ProjectManager.Main.Controllers;

/// <summary>
/// Контроллер для проверки состояния микросервиса.
/// Предоставляет метод для проверки доступности сервиса.

[ApiController]
[Route("api/[controller]")]

public class HealthChecker : Controller
{
    [HttpGet("health")]
    public IActionResult HealthCheck()
    {
        return Ok();
    }
}