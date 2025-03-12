using Microsoft.AspNetCore.Http;

namespace ProjectManager.Application.Interfaces.ValidationInterfaces
{
    /// <summary>
    /// Валидатор файлов.
    /// </summary>
    public interface IFileValidator
    {
        /// <summary>
        /// Проверяет, является ли файл допустимым
        /// </summary>
        /// <param name="file">Файл для проверки</param>
        /// <returns>True, если файл допустим, иначе False</returns>
        bool IsValid(IFormFile file);
    }   
}
