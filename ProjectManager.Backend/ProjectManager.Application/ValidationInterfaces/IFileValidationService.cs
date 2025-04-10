using Microsoft.AspNetCore.Http;

namespace ProjectManager.Application.ValidationInterfaces;

public interface IFileValidationService
{
    /// <summary>
    /// Проверяет загружаемый аватар на соответствие требованиям.
    /// </summary>
    /// <param name="file">Файл изображения для проверки.</param>
    Task ValidateFilesAsync(IFormFile[] files);
}