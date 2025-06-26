using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AuthService.Core.Interfaces;
using AuthService.Core.Models;
using AuthService.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Http;

namespace AuthService.Application.Services;

public class AuthService : IAuthService
{
    private readonly IConfiguration _configuration;
    private readonly IUserService _userService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMessagePublisher _messagePublisher;

    public AuthService(
        IConfiguration configuration, 
        IUserService userService,
        IHttpContextAccessor httpContextAccessor,
        IMessagePublisher messagePublisher)
    {
        _configuration = configuration;
        _userService = userService;
        _httpContextAccessor = httpContextAccessor;
        _messagePublisher = messagePublisher;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        var user = await _userService.CreateUserAsync(request);
        if (user == null)
        {
            return new AuthResponse { Success = false, Message = "Failed to create user" };
        }

        try
        {
            var message = new Shared.Messages.Models.UserCreatedMessage
            {
                UserId = user.Id,
                Email = user.Email,
                Username = user.Username,
                CreatedAt = DateTime.UtcNow
            };

            await _messagePublisher.PublishUserCreatedAsync(message);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error publishing user created message: {ex.Message}");
        }

        return await GenerateAuthResponseAsync(user);
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var user = await _userService.ValidateUserAsync(request.Email, request.Password);
        if (user == null)
        {
            return new AuthResponse { Success = false, Message = "Invalid credentials" };
        }

        return await GenerateAuthResponseAsync(user);
    }

    public async Task<AuthResponse> RefreshTokenAsync(RefreshTokenRequest request)
    {
        var user = await _userService.GetUserByRefreshTokenAsync(request.RefreshToken);
        if (user == null)
        {
            return new AuthResponse { Success = false, Message = "Invalid refresh token" };
        }

        return await GenerateAuthResponseAsync(user);
    }

    public async Task<bool> RevokeTokenAsync(string refreshToken)
    {
        return await _userService.RevokeRefreshTokenAsync(refreshToken);
    }

    public async Task<UserClaims> GetUserClaimsAsync(string userId)
    {
        var user = await _userService.GetUserByIdAsync(userId);
        if (user == null)
        {
            return new UserClaims();
        }

        return new UserClaims
        {
            UserId = user.Id,
            Email = user.Email,
            Username = user.Username,
            Roles = user.UserRoles.Select(ur => ur.Role.Name).ToList()
        };
    }

    private async Task<AuthResponse> GenerateAuthResponseAsync(User user)
    {
        var token = GenerateJwtToken(user);
        var refreshToken = await _userService.GenerateRefreshTokenAsync(user.Id);

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddHours(Convert.ToDouble(_configuration["Jwt:ExpireHours"] ?? "1"))
        };

        _httpContextAccessor.HttpContext?.Response.Cookies.Append("jwt", token, cookieOptions);

        var refreshCookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddDays(7)
        };

        _httpContextAccessor.HttpContext?.Response.Cookies.Append("refreshToken", refreshToken, refreshCookieOptions);

        return new AuthResponse
        {
            Success = true,
            Message = "Authentication successful",
            Token = token,
            RefreshToken = refreshToken
        };
    }

    private string GenerateJwtToken(User user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),

            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Name, user.Username)
        };

        foreach (var userRole in user.UserRoles)
        {
            claims.Add(new Claim("role", userRole.Role.Name));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key not found")));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.Now.AddHours(Convert.ToDouble(_configuration["Jwt:ExpireHours"] ?? "1"));

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: expires,
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
} 