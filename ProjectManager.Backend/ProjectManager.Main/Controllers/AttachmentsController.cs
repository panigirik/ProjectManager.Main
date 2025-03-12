using Microsoft.AspNetCore.Mvc;
using ProjectManager.Application.DTOs;
using ProjectManager.Application.Interfaces;
using ProjectManager.Domain.Entities;

namespace ProjectManager.Main.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AttachmentsController : ControllerBase
{
    private readonly IAttachmentService _attachmentService;

    public AttachmentsController(IAttachmentService attachmentService)
    {
        _attachmentService = attachmentService;
    }

    // GET: api/attachments
    [HttpGet("attachments")]
    public async Task<IActionResult> GetAll()
    {
        var attachments = await _attachmentService.GetAllAsync();
        return Ok(attachments);
    }

    // GET: api/attachments/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var attachment = await _attachmentService.GetByIdAsync(id);
        if (attachment == null)
        {
            return NotFound();
        }
        return Ok(attachment);
    }

    // POST: api/attachments
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AttachmentDto attachmentDto)
    {
        if (attachmentDto == null)
        {
            return BadRequest("Attachment data is null.");
        }

        await _attachmentService.CreateAsync(attachmentDto);
        return CreatedAtAction(nameof(GetById), new { id = attachmentDto.AttachmentId }, attachmentDto);
    }

    // PUT: api/attachments
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] AttachmentDto attachmentDto)
    {
        if (attachmentDto == null)
        {
            return BadRequest("Attachment data is null.");
        }

        await _attachmentService.UpdateAsync(attachmentDto);
        return NoContent();
    }

    // DELETE: api/attachments/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _attachmentService.DeleteAsync(id);
        return NoContent();
    }
}