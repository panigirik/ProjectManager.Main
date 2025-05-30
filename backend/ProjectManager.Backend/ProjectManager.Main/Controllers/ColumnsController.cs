﻿using Microsoft.AspNetCore.Mvc;
using ProjectManager.Application.DTOs;
using ProjectManager.Application.Interfaces;
using ProjectManager.Application.RequestsDTOs;

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
    
    [HttpGet("column")]
    public async Task<IActionResult> GetAll()
    {
        var columns = await _columnService.GetAllAsync();
        return Ok(columns);
    }
    
    [HttpGet("column/{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var column = await _columnService.GetByIdAsync(id);
        if (column == null)
        {
            return NotFound();
        }
        return Ok(column);
    }

    [HttpGet("columns/{boardId}")]
    public async Task<IActionResult> GetColumnsByBoardId(Guid boardId)
    {
        var columns = await _columnService.GetColumnsByBoardIdAsync(boardId);
        return Ok(columns);
    }
    
    [HttpPost("column")]
    public async Task<IActionResult> Create([FromBody] ColumnDto columnDto)
    {
        if (columnDto == null)
        {
            return BadRequest("Column data is null.");
        }

        await _columnService.CreateAsync(columnDto);
        return CreatedAtAction(nameof(GetById), new { id = columnDto.ColumnId }, columnDto);
    }
    
    [HttpPut("column")]
    public async Task<IActionResult> Update([FromForm] UpdateColumnRequest updateColumnRequest)
    {

        await _columnService.UpdateAsync(updateColumnRequest);
        return NoContent();
    }
    
    [HttpDelete("column/{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _columnService.DeleteAsync(id);
        return NoContent();
    }
}