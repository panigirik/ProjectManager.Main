using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using ProjectManager.Application.ValidationInterfaces;
using ProjectManager.ExternalServices.Services.ClamAV.Helpers;
using ValidationException = System.ComponentModel.DataAnnotations.ValidationException;
using SixLabors.ImageSharp;

namespace ProjectManager.ValidationServices.Services
{
    public class FileValidationService : IFileValidationService
    {
        private readonly IValidator<IFormFile> _fileValidator;
        private readonly int _maxFileSizeMb;
        private readonly int _maxResolution;
        private readonly string[] _allowedFormats;
        private readonly string[] _forbiddenFormats;
        private readonly ScanFileForMalwareHelper _malwareHelper;

        /// <summary>
        /// Конструктор сервиса, загружает настройки валидации из конфигурации.
        /// </summary>
        public FileValidationService(IValidator<IFormFile> fileValidator,
            IConfiguration configuration,
            ScanFileForMalwareHelper malwareHelper)
        {
            _fileValidator = fileValidator;
            _maxFileSizeMb = int.Parse(configuration["AvatarValidation:MaxFileSizeMB"] ?? throw new InvalidOperationException("MaxFileSizeMB is empty"));
            _maxResolution = int.Parse(configuration["AvatarValidation:MaxResolution"] ?? throw new InvalidOperationException("MaxResolution is empty"));
            _allowedFormats = configuration.GetSection("AvatarValidation:AllowedFormats").Get<string[]>() ?? Array.Empty<string>();
            _forbiddenFormats = configuration.GetSection("AvatarValidation:ForbiddenFormats").Get<string[]>() ?? Array.Empty<string>();
            _malwareHelper = malwareHelper;
        }

        /// <summary>
        /// Проверяет загружаемый аватар на соответствие требованиям.
        /// </summary>
        /// <param name="file">Файл изображения для проверки.</param>
        public async Task ValidateFileAsync(IFormFile file)
        {
            var validationResult = await _fileValidator.ValidateAsync(file);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors.ToString());
            }

            ValidateFileFormat(file);
            ValidateFileSize(file);
            await ValidateImageResolutionAsync(file);
            await _malwareHelper.TestConnectionAsync();
            await _malwareHelper.ScanFileAsync(file);

        }

        /// <summary>
        /// Проверяет формат загружаемого файла.
        /// </summary>
        private void ValidateFileFormat(IFormFile file)
        {
            string extension = Path.GetExtension(file.FileName).ToLower();
            if (_forbiddenFormats.Contains(extension))
            {
                throw new ValidationException($"File format {extension} not allowed");
            }
            if (!_allowedFormats.Contains(extension))
            {
                throw new ValidationException($"Not allowed format {extension}");
            }
        }

        /// <summary>
        /// Проверяет размер загружаемого файла.
        /// </summary>
        private void ValidateFileSize(IFormFile file)
        {
            if (file.Length > _maxFileSizeMb * 1024 * 1024)
            {
                throw new ValidationException($"File size is bigger than {_maxFileSizeMb}MB.");
            }
        }

        /// <summary>
        /// Проверяет разрешение изображения.
        /// </summary>
        private async Task ValidateImageResolutionAsync(IFormFile file)
        {
            using var stream = file.OpenReadStream();
            var image = await Image.LoadAsync(stream); 
            if (image.Width > _maxResolution || image.Height > _maxResolution)
            {
                throw new ValidationException($"Resolution is bigger than {_maxResolution}x{_maxResolution}");
            }
        }
    }
}
