using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.DTOs.Auth;
using RealEstate.Domain.Interfaces.Services;
using System.Security.Claims;

namespace RealEstate.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService) => _authService = authService;

    /// <summary>Register a new Seller or Buyer</summary>
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        var result = await _authService.RegisterAsync(dto.Email, dto.Password, dto.FirstName, dto.LastName, dto.Role);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>Login and receive JWT + RefreshToken</summary>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var result = await _authService.LoginAsync(dto.Email, dto.Password);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>Refresh an expired JWT</summary>
    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] string refreshToken)
    {
        var result = await _authService.RefreshTokenAsync(refreshToken);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>Logout — invalidate refresh token</summary>
    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";
        var result = await _authService.LogoutAsync(userId);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>Get current user profile</summary>
    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> Me()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";
        var result = await _authService.GetCurrentUserAsync(userId);
        return StatusCode(result.StatusCode, result);
    }
}
