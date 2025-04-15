using FluentValidation;
using FluentValidation.Results;
using ProjectManager.Application.DTOs;
using ProjectManager.Application.Exceptions;
using ProjectManager.Application.Interfaces.ValidationInterfaces;
using ProjectManager.ValidationServices.ValidateRules;

namespace ProjectManager.ValidationServices.Services;

/// <summary>
/// Сервис валидации при обновлении ифнормации пользователя.
/// </summary>
public class UserDtoValidationService: IUserDtoValidationService
{
    private readonly AbstractValidator<UserDto> _validator;
    
    public UserDtoValidationService()
    {
        _validator = new UserDtoValidator();
    }
    
    /// <summary>
    /// Выполняет валидацию при создании пользователя.
    /// </summary>
    public async Task<ValidationResult> ValidateUserDtoAsync(UserDto userDto)
    {
        var validationResult = await _validator.ValidateAsync(userDto);
            if (!validationResult.IsValid)
            {
                throw new BadRequestException("bad request");
            }

            return validationResult;
    }

}