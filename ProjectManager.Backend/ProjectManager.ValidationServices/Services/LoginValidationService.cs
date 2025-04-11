using FluentValidation;
using FluentValidation.Results;
using ProjectManager.Application.Interfaces.ValidationInterfaces;
using ProjectManager.Application.RequestsDTOs;

namespace ProjectManager.ValidationServices.Services;

/// <summary>
/// Сервис для выполнения use-case валидации запросов и пользователей.
/// </summary>
public class LoginValidationService : ILoginValidationService
{
    private readonly IValidator<LoginRequest> _loginRequestValidator;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="ValidationUseCase"/>.
    /// </summary>
    /// <param name="loginRequestValidator">Валидатор для запросов логина.</param>
    public LoginValidationService(IValidator<LoginRequest> loginRequestValidator)
    {
        _loginRequestValidator = loginRequestValidator;
    }

    /// <summary>
    /// Выполняет валидацию запроса на логин.
    /// </summary>
    /// <param name="request">Запрос на логин.</param>
    /// <returns>Результат валидации.</returns>
    public async Task<ValidationResult> ValidateLoginRequestAsync(LoginRequest request)
    {
        var result = await _loginRequestValidator.ValidateAsync(request);
        

        return result;
    }


}