using FluentValidation;
using ProjectManager.Application.DTOs;
using ProjectManager.Application.Exceptions;
using ProjectManager.Application.Interfaces.ValidationInterfaces;
using ProjectManager.ValidationServices.ValidateRules;

namespace ProjectManager.ValidationServices.Services;

/// <summary>
/// UseCase валидации при добавлении пользователя.
/// </summary>
public class AddUserValidationService : IAddUserValidationService
{
    private readonly AbstractValidator<UserDto> _validator;
    
    public AddUserValidationService()
    {
        _validator = new UserDtoValidator();
    }

    /// <summary>
    /// Выполняет валидацию при создании пользователя.
    /// </summary>
    public async Task ValidateAsync(UserDto userDto)
    {
        var validationResult = await _validator.ValidateAsync(userDto);

        if (!validationResult.IsValid)
        {
            throw new BadRequestException("bad request");
        }
    }
}