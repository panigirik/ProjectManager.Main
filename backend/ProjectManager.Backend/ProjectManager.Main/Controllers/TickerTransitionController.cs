using Microsoft.AspNetCore.Mvc;
using ProjectManager.Application.Interfaces;
using ProjectManager.Application.RequestsDTOs;

namespace ProjectManager.Main.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TicketTransitionController : ControllerBase
{
    private readonly ITicketTransitionService _transitionService;
    
    public TicketTransitionController(ITicketTransitionService transitionService)
    {
        _transitionService = transitionService;
    }
    
    [HttpPost("rule")]
    public async Task<IActionResult> AddRule([FromBody] CreateTransitionRuleRequest request)
    {
        var result = await _transitionService.AddTransitionRuleAsync(request);
        return CreatedAtAction(nameof(AddRule), new { id = result.TicketTransitionRuleId }, result);
    }
}