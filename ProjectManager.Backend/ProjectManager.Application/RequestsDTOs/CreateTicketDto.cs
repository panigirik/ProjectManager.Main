using Microsoft.AspNetCore.Http;

namespace ProjectManager.Application.RequestsDTOs;

public class CreateTicketDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string AssignedUserName { get; set; }
    public Guid ColumnId { get; set; }
    public IFormFile[]? Attachments { get; set; }
}
