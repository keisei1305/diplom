using AuthService.Core.Interfaces;
using AuthService.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthServiceApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register(RegisterRequest request)
    {
        var response = await _authService.RegisterAsync(request);
        if (!response.Success)
        {
            return BadRequest(response);
        }
        return Ok(response);
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login(LoginRequest request)
    {
        var response = await _authService.LoginAsync(request);
        if (!response.Success)
        {
            return Unauthorized(response);
        }
        return Ok(response);
    }

    [HttpPost("refresh-token")]
    public async Task<ActionResult<AuthResponse>> RefreshToken(RefreshTokenRequest request)
    {
        var response = await _authService.RefreshTokenAsync(request);
        if (!response.Success)
        {
            return Unauthorized(response);
        }
        return Ok(response);
    }

    [Authorize]
    [HttpPost("revoke-token")]
    public async Task<ActionResult> RevokeToken([FromBody] RefreshTokenRequest request)
    {
        var result = await _authService.RevokeTokenAsync(request.RefreshToken);
        if (!result)
        {
            return BadRequest("Invalid refresh token");
        }
        return Ok();
    }

    [Authorize]
    [HttpGet("claims")]
    public async Task<ActionResult<UserClaims>> GetUserClaims()
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var claims = await _authService.GetUserClaimsAsync(userId);
        return Ok(claims);
    }
} 