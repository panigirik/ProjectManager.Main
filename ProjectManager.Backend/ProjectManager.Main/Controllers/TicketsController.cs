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
    private readonly IDropBoxClient _dropBoxClient;
    
    public TicketsController(ITicketService ticketService,
        IFileValidationService fileValidationService,
        IDropBoxClient dropBoxClient)
    {
        _ticketService = ticketService;
        _fileValidationService = fileValidationService;
        _dropBoxClient = dropBoxClient;
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
    
    [HttpPost("ticket")]
    public async Task<IActionResult> Create([FromForm] CreateTicketDto ticketDto)
    {
        if (ticketDto.Attachments != null)
        {
            await _fileValidationService.ValidateFilesAsync(ticketDto.Attachments);
        }
        
        var newTicket = await _ticketService.CreateTicketAsync(ticketDto);
        return Ok(newTicket);
    }
    
    [HttpPut("ticket")]
    public async Task<IActionResult> Update([FromBody] UpdateTicketDto ticketDto)
    {
        if (ticketDto == null)
        {
            return BadRequest("Ticket data is null.");
        }

        await _fileValidationService.ValidateFilesAsync(ticketDto.Attachments);
        await _ticketService.UpdateAsync(ticketDto);
        return NoContent();
    }
    
    [HttpPut("move-ticket")]
    public async Task<IActionResult> MoveTicket([FromForm] MoveTicketRequest request)
    {
        if (request.TicketId == Guid.Empty || request.NewColumnId == Guid.Empty)
        {
            return BadRequest("TicketId or NewColumnId is invalid.");
        }



        try
        {
            // Передаем все входные данные в метод перемещения
            await _ticketService.MoveToColumn(request);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }


    [HttpDelete("ticket/{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _ticketService.DeleteAsync(id);
        return NoContent();
    }
}