using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ProjectManager.Application.DTOs;

public class UserDto
{
    public Guid UserId { get; set; } 
    public string UserName { get; set; }
    public string Email { get; set; }
    public List<Guid> BoardIds { get; set; } = new(); 
}