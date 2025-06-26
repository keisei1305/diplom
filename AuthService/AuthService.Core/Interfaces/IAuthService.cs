using AuthService.Core.Models;

namespace AuthService.Core.Interfaces;

public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(RegisterRequest request);
    Task<AuthResponse> LoginAsync(LoginRequest request);
    Task<AuthResponse> RefreshTokenAsync(RefreshTokenRequest request);
    Task<bool> RevokeTokenAsync(string refreshToken);
    Task<UserClaims> GetUserClaimsAsync(string userId);
} 