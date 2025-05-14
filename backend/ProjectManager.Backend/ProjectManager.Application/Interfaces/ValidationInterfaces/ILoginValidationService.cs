
using FluentValidation.Results;
using ProjectManager.Application.RequestsDTOs;

namespace ProjectManager.Application.Interfaces.ValidationInterfaces;

/// <summary>
/// Интерфейс для валидации запросов и пользователей.
/// </summary>
public interface ILoginValidationService
{
    /// <summary>
    /// Асинхронно выполняет валидацию запроса на вход в систему.
    /// </summary>
    /// <param name="request">Данные запроса для входа.</param>
    /// <returns>Результат валидации.</returns>
    Task<ValidationResult> ValidateLoginRequestAsync(LoginRequest request);

}