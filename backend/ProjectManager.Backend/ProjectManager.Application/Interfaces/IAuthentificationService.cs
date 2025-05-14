using ProjectManager.Application.RequestsDTOs;

namespace ProjectManager.Application.Interfaces;

public interface IAuthentificationService
{
    Task<LoginResult> LoginAsync(LoginRequest request);
    
    Task LogoutAsync(Guid userId);
    
    Task<string> RefreshTokenAsync(Guid userId, string refreshToken);
    
    Task RevokeRefreshTokenAsync(Guid id);
}