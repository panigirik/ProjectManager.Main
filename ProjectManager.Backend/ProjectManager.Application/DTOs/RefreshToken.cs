using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ProjectManager.Application.DTOs;


/// <summary>
/// DTO (Data Transfer Object) для представления обновляемого токена (refresh token).
/// </summary>
public class RefreshTokenDto
{
    /// <summary>
    /// Уникальный идентификатор обновляемого токена.
    /// </summary>
    [BsonRepresentation(BsonType.String)]
    public Guid RefreshTokenId { get; set; }

    /// <summary>
    /// Идентификатор пользователя, которому принадлежит токен.
    /// </summary>
    [BsonRepresentation(BsonType.String)]
    public Guid UserId { get; set; }

    /// <summary>
    /// Сам обновляемый токен.
    /// </summary>
    public string Token { get; set; }

    /// <summary>
    /// Дата истечения срока действия токена.
    /// </summary>
    public DateTime Expires { get; set; }

    /// <summary>
    /// Флаг, указывающий, был ли токен отозван.
    /// </summary>
    public bool IsRevoked { get; set; }
}