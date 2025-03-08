using Microsoft.AspNetCore.Mvc;
using ProjectManager.Application.DTOs;
using ProjectManager.Application.Interfaces;

namespace ProjectManager.Main.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ColumnsController : ControllerBase
{
    private readonly IColumnService _columnService;

    public ColumnsController(IColumnService columnService)
    {
        _columnService = columnService;
    }

    // GET: api/columns
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var columns = await _columnService.GetAllAsync();
        return Ok(columns);
    }

    // GET: api/columns/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var column = await _columnService.GetByIdAsync(id);
        if (column == null)
        {
            return NotFound();
        }
        return Ok(column);
    }

    // POST: api/columns
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ColumnDto columnDto)
    {
        if (columnDto == null)
        {
            return BadRequest("Column data is null.");
        }

        await _columnService.CreateAsync(columnDto);
        return CreatedAtAction(nameof(GetById), new { id = columnDto.ColumnId }, columnDto);
    }

    // PUT: api/columns
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] ColumnDto columnDto)
    {
        if (columnDto == null)
        {
            return BadRequest("Column data is null.");
        }

        await _columnService.UpdateAsync(columnDto);
        return NoContent();
    }

    // DELETE: api/columns/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        await _columnService.DeleteAsync(id);
        return NoContent();
    }
}