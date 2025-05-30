﻿using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ProjectManager.Domain.Entities;

public class Board
{
    /// <summary>
    /// Уникальный идентификатор документа.
    /// </summary>
    [BsonId]
    [NotMapped]
    public ObjectId Id { get; set; } 
    
    [BsonRepresentation(BsonType.String)]
    public Guid? BoardId { get; set; } 

    [BsonRepresentation(BsonType.String)]
    public Guid? CreatorId { get; set; } 
    
    public string BoardName { get; set; }
    
    [BsonRepresentation(BsonType.String)]
    public List<Guid>? ColumnIds { get; set; } = new();
}
