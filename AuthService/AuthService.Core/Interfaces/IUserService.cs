using AuthService.Core.Models;

namespace AuthService.Core.Interfaces;

public interface IUserService
{
    Task<User?> CreateUserAsync(RegisterRequest request);
    Task<User?> ValidateUserAsync(string email, string password);
    Task<User?> GetUserByIdAsync(string userId);
    Task<User?> GetUserByRefreshTokenAsync(string refreshToken);
    Task<string> GenerateRefreshTokenAsync(string userId);
    Task<bool> RevokeRefreshTokenAsync(string refreshToken);
} 