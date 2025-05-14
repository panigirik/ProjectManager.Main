using FluentValidation.Results;
using ProjectManager.Application.DTOs;

namespace ProjectManager.Application.Interfaces.ValidationInterfaces;

/// <summary>
/// Интерфейс для валидации запросов и пользователей.
/// </summary>
public interface IUserDtoValidationService
{
    /// <summary>
    /// Асинхронно выполняет валидацию запроса на изменение данных пользователя.
    /// </summary>
    /// <param name="userDto">Данные запроса.</param>
    /// <returns>Результат валидации.</returns>
    Task<ValidationResult> ValidateUserDtoAsync(UserDto userDto);
}