using FluentValidation;
using Microsoft.AspNetCore.Http;
using ProjectManager.Application.Interfaces.ValidationInterfaces;

namespace ProjectManager.ValidationServices.ValidateRules;

public class FileFormValidator : AbstractValidator<IFormFile>
{
    public FileFormValidator()
    {
        RuleFor(file => file.Length).GreaterThan(0).WithMessage("File must not be empty.");
        RuleFor(file => file.Length).LessThanOrEqualTo(10 * 1024 * 1024).WithMessage("File size must not exceed 10 MB.");
        RuleFor(file => file.ContentType).Must(contentType => contentType.StartsWith("image/")).WithMessage("Only image files are allowed.");
    }
}