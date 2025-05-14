using Microsoft.AspNetCore.Mvc;
using ProjectManager.Application.DTOs;
using ProjectManager.Application.Interfaces;

namespace ProjectManager.Main.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BoardsController : ControllerBase
{
    private readonly IBoardService _boardService;

    public BoardsController(IBoardService boardService)
    {
        _boardService = boardService;
    }
    
    [HttpGet("column")]
    public async Task<IActionResult> GetAll()
    {
        var boards = await _boardService.GetAllAsync();
        return Ok(boards);
    }
    
    [HttpGet("column/{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var board = await _boardService.GetByIdAsync(id);
        if (board == null)
        {
            return NotFound();
        }
        return Ok(board);
    }
    
    [HttpPost("column")]
    public async Task<IActionResult> Create([FromBody] BoardDto boardDto)
    {
        if (boardDto == null)
        {
            return BadRequest("Board data is null.");
        }

        await _boardService.CreateAsync(boardDto);
        return CreatedAtAction(nameof(GetById), new { id = boardDto.BoardId }, boardDto);
    }
    
    [HttpPut("column")]
    public async Task<IActionResult> Update([FromBody] BoardDto boardDto)
    {
        await _boardService.UpdateAsync(boardDto);
        return NoContent();
    }
    
    [HttpDelete("column/{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _boardService.DeleteAsync(id);
        return NoContent();
    }
}