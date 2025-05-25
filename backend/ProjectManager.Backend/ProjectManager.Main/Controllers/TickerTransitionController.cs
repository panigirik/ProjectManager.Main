using Microsoft.AspNetCore.Mvc;
using Netway.Utils.Interfaces;
using ProjectManager.Application.Interfaces;
using ProjectManager.Application.RequestsDTOs;
using Serilog;

namespace ProjectManager.Main.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TicketTransitionController : ControllerBase
{
    private readonly ITicketTransitionService _transitionService;
    private readonly IUserHelperService _userHelperService;
    private readonly IBoardService _boardService;
    
    public TicketTransitionController(ITicketTransitionService transitionService,
        IUserHelperService userHelperService,
        IBoardService boardService)
    {
        _transitionService = transitionService;
        _userHelperService = userHelperService;
        _boardService = boardService;
    }
    
    [HttpPost("rule")]
    public async Task<IActionResult> AddRule([FromBody] CreateTransitionRuleRequest request)
    {
        var currentUser = _userHelperService.GetCurrentUser();
        
        var board = await _boardService.GetBoardByTicketIdAsync(request.TicketId);
        var userId = _userHelperService.GetCurrentUser().UserId;
        Log.Information("current UserId" + userId);
        if (board == null)
            return NotFound("Board not found");

        if (board.CreatorId != currentUser.UserId)
            return Forbid("Only the board creator can add transition rules.");

        var result = await _transitionService.AddTransitionRuleAsync(request);
        return CreatedAtAction(nameof(AddRule), new { id = result.TicketTransitionRuleId }, result);
    }

}