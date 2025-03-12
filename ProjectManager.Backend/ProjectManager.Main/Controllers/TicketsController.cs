using Microsoft.AspNetCore.Mvc;
using ProjectManager.Application.DTOs;
using ProjectManager.Application.Interfaces;

namespace ProjectManager.Main.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TicketsController : ControllerBase
{
    private readonly ITicketService _ticketService;

    public TicketsController(ITicketService ticketService)
    {
        _ticketService = ticketService;
    }

    // GET: api/tickets
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var tickets = await _ticketService.GetAllAsync();
        return Ok(tickets);
    }

    // GET: api/tickets/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var ticket = await _ticketService.GetByIdAsync(id);
        if (ticket == null)
        {
            return NotFound();
        }
        return Ok(ticket);
    }

    // POST: api/tickets
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TicketDto ticketDto)
    {
        if (ticketDto == null)
        {
            return BadRequest("Ticket data is null.");
        }

        await _ticketService.CreateAsync(ticketDto);
        return CreatedAtAction(nameof(GetById), new { id = ticketDto.TicketId }, ticketDto);
    }

    // PUT: api/tickets
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] TicketDto ticketDto)
    {
        if (ticketDto == null)
        {
            return BadRequest("Ticket data is null.");
        }

        await _ticketService.UpdateAsync(ticketDto);
        return NoContent();
    }

    // DELETE: api/tickets/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _ticketService.DeleteAsync(id);
        return NoContent();
    }
}