using Microsoft.AspNetCore.Mvc;
using ProjectManager.Application.DTOs;
using ProjectManager.Application.Interfaces;
using ProjectManager.Application.RequestsDTOs;
using ProjectManager.Application.ValidationInterfaces;
using ProjectManager.Domain.Interfaces.ExternalServices;
using ProjectManager.ExternalServices.Services.CloudStorageServices;

namespace ProjectManager.Main.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TicketsController : ControllerBase
{
    private readonly ITicketService _ticketService;
    private readonly IFileValidationService _fileValidationService;
    
    public TicketsController(ITicketService ticketService,
        IFileValidationService fileValidationService)
    {
        _ticketService = ticketService;
        _fileValidationService = fileValidationService;
    }
    
    [HttpGet("ticket")]
    public async Task<IActionResult> GetAll()
    {
        var tickets = await _ticketService.GetAllAsync();
        return Ok(tickets);
    }

    [HttpGet("ticket/{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var ticket = await _ticketService.GetByIdAsync(id);
        if (ticket == null)
        {
            return NotFound();
        }
        return Ok(ticket);
    }

    [HttpGet("column/{columnId}")]
    public async Task<IActionResult> GetTicketsByColumn(Guid columnId)
    {
        var tickets = await _ticketService.GetTicketsByColumnIdAsync(columnId);
        return Ok(tickets);
    }
    
    [HttpPost("ticket")]
    public async Task<IActionResult> Create([FromForm] CreateTicketRequest ticketRequest)
    {
        if (ticketRequest.Attachments != null)
        {
            await _fileValidationService.ValidateFilesAsync(ticketRequest.Attachments);
        }
        
        var newTicket = await _ticketService.CreateTicketAsync(ticketRequest);
        return Ok(newTicket);
    }
    
    [HttpPut("ticket")]
    public async Task<IActionResult> Update([FromForm] UpdateTicketRequest ticketRequest)
    {
        if (ticketRequest == null)
        {
            return BadRequest("Ticket data is null.");
        }
        
        await _ticketService.UpdateAsync(ticketRequest);
        return NoContent();
    }
    
    [HttpGet("ticket/{ticketId}/attachments/links")]
    public async Task<IActionResult> GetTicketAttachmentLinks(Guid ticketId)
    {
        var links = await _ticketService.GetAttachmentsPathsAsync(ticketId);

        return Ok(links);
    }

    
    [HttpPut("move-ticket")]
    public async Task<IActionResult> MoveTicket([FromBody] MoveTicketRequest request)
    {
        if (request.TicketId == Guid.Empty || request.NewColumnId == Guid.Empty)
        {
            return BadRequest("TicketId or NewColumnId is invalid.");
        }
        try
        {
            await _ticketService.MoveToColumnAsync(request);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message); // ← вот сюда должно попадать
        }
    }



    [HttpDelete("ticket/{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _ticketService.DeleteAsync(id);
        return NoContent();
    }
    
}