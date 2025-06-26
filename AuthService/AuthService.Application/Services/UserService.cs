using System.Security.Cryptography;
using System.Text;
using AuthService.Core.Interfaces;
using AuthService.Core.Models;

namespace AuthService.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User?> CreateUserAsync(RegisterRequest request)
    {
        var existingUser = await _userRepository.GetByEmailAsync(request.Email);
        if (existingUser != null)
        {
            return null;
        }

        var user = new User
        {
            Username = request.Username,
            Email = request.Email,
            PasswordHash = HashPassword(request.Password)
        };

        user.UserRoles.Add(new UserRole { RoleId = "2" });

        await _userRepository.AddAsync(user);

        var createdUser = await _userRepository.GetByEmailAsync(request.Email);
        return createdUser;
    }

    public async Task<User?> ValidateUserAsync(string email, string password)
    {
        var user = await _userRepository.GetByEmailAsync(email);
        if (user == null)
        {
            return null;
        }

        return VerifyPassword(password, user.PasswordHash) ? user : null;
    }

    public async Task<User?> GetUserByIdAsync(string userId)
    {
        return await _userRepository.GetByIdAsync(userId);
    }

    public async Task<User?> GetUserByRefreshTokenAsync(string refreshToken)
    {
        return await _userRepository.GetByRefreshTokenAsync(refreshToken);
    }

    public async Task<string> GenerateRefreshTokenAsync(string userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
        {
            throw new ArgumentException("User not found", nameof(userId));
        }

        var refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

        await _userRepository.UpdateAsync(user);
        return refreshToken;
    }

    public async Task<bool> RevokeRefreshTokenAsync(string refreshToken)
    {
        var user = await _userRepository.GetByRefreshTokenAsync(refreshToken);
        if (user == null)
        {
            return false;
        }

        user.RefreshToken = null;
        user.RefreshTokenExpiryTime = null;
        await _userRepository.UpdateAsync(user);
        return true;
    }

    private static byte[] HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        return sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
    }

    private static bool VerifyPassword(string password, byte[] hash)
    {
        var hashOfInput = HashPassword(password);
        return CryptographicOperations.FixedTimeEquals(hashOfInput, hash);
    }
} 