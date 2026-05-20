using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using RealEstate.Application.DTOs.Auth;
using RealEstate.Domain.Common;
using RealEstate.Domain.Entities.User;
using RealEstate.Domain.Interfaces.Services;

namespace RealEstate.Application.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly ILogger<AuthService> _logger;
    // JwtTokenGenerator injected via interface — defined in Infrastructure
    private readonly Func<AppUser, Task<TokenResponseDto>> _generateToken;

    public AuthService(
        UserManager<AppUser> userManager,
        ILogger<AuthService> logger,
        Func<AppUser, Task<TokenResponseDto>> generateToken)
    {
        _userManager = userManager;
        _logger = logger;
        _generateToken = generateToken;
    }

    public async Task<GenericResponse<object>> RegisterAsync(string email, string password, string firstName, string lastName, string role)
    {
        var allowed = new[] { "Seller", "Buyer" };
        if (!allowed.Contains(role)) return GenericResponse<object>.Failure("Role must be Seller or Buyer");

        var existing = await _userManager.FindByEmailAsync(email);
        if (existing is not null) return GenericResponse<object>.Failure("Email already registered");

        var user = new AppUser { UserName = email, Email = email, FirstName = firstName, LastName = lastName };
        var result = await _userManager.CreateAsync(user, password);
        if (!result.Succeeded)
            return GenericResponse<object>.Failure("Registration failed", result.Errors.Select(e => e.Description).ToList());

        await _userManager.AddToRoleAsync(user, role);
        _logger.LogInformation("User registered: {Email} as {Role}", email, role);
        return GenericResponse<object>.Success(new { user.Id, user.Email, role }, "Registered successfully", 201);
    }

    public async Task<GenericResponse<object>> LoginAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null || !await _userManager.CheckPasswordAsync(user, password))
            return GenericResponse<object>.Unauthorized("Invalid email or password");

        if (user.IsBlocked)
            return GenericResponse<object>.Unauthorized("Your account has been blocked");

        var token = await _generateToken(user);
        user.RefreshToken = token.RefreshToken;
        user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
        await _userManager.UpdateAsync(user);

        return GenericResponse<object>.Success(token, "Login successful");
    }

    public async Task<GenericResponse<object>> RefreshTokenAsync(string refreshToken)
    {
        var user = _userManager.Users.FirstOrDefault(u => u.RefreshToken == refreshToken);
        if (user is null || user.RefreshTokenExpiry < DateTime.UtcNow)
            return GenericResponse<object>.Unauthorized("Invalid or expired refresh token");

        var token = await _generateToken(user);
        user.RefreshToken = token.RefreshToken;
        user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
        await _userManager.UpdateAsync(user);
        return GenericResponse<object>.Success(token);
    }

    public async Task<GenericResponse<object>> LogoutAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null) return GenericResponse<object>.NotFound();
        user.RefreshToken = null;
        user.RefreshTokenExpiry = null;
        await _userManager.UpdateAsync(user);
        return GenericResponse<object>.Success(new { }, "Logged out");
    }

    public async Task<GenericResponse<object>> GetCurrentUserAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null) return GenericResponse<object>.NotFound();
        var roles = await _userManager.GetRolesAsync(user);
        return GenericResponse<object>.Success(new
        {
            user.Id, user.Email, user.FirstName, user.LastName,
            Role = roles.FirstOrDefault(), user.CreatedAt
        });
    }
}
