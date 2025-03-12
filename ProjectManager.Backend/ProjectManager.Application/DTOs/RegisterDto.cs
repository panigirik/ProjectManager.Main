namespace ProjectManager.Application.DTOs
{
    public class RegisterDto
    {
        public string UserName { get; set; }  // Имя пользователя
        public string Email { get; set; }     // Электронная почта пользователя
        public string Password { get; set; }  // Пароль пользователя (в виде строки)
    }
}