using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ProjectManager.Domain.Entities;

public class Column
{
    /// <summary>
    /// Уникальный идентификатор документа.
    /// </summary>
    [BsonId]
    [NotMapped]
    public ObjectId Id { get; set; } 
    
    [BsonRepresentation(BsonType.String)]
    public Guid ColumnId { get; set; } 
    public string ColumnName { get; set; }
    
    [BsonRepresentation(BsonType.String)]
    public Guid BoardId { get; set; } 
    
    [BsonRepresentation(BsonType.String)]
    public List<Guid> TicketIds { get; set; } = new(); // Храним ссылки на тикеты
}
