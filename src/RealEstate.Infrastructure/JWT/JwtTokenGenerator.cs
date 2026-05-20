using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RealEstate.Application.DTOs.Auth;
using RealEstate.Domain.Entities.User;

namespace RealEstate.Infrastructure.JWT;

public class JwtTokenGenerator
{
    private readonly JwtSettings _settings;
    private readonly UserManager<AppUser> _userManager;

    public JwtTokenGenerator(IOptions<JwtSettings> settings, UserManager<AppUser> userManager)
    {
        _settings = settings.Value;
        _userManager = userManager;
    }

    public async Task<TokenResponseDto> GenerateTokenAsync(AppUser user)
    {
        var roles  = await _userManager.GetRolesAsync(user);
        var role   = roles.FirstOrDefault() ?? "";
        var expiry = DateTime.UtcNow.AddMinutes(_settings.ExpiryMinutes);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub,   user.Id),
            new(JwtRegisteredClaimNames.Email, user.Email ?? ""),
            new(JwtRegisteredClaimNames.Jti,   Guid.NewGuid().ToString()),
            new(ClaimTypes.Role,               role),
            new(ClaimTypes.NameIdentifier,     user.Id)
        };

        var key   = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.SecretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer:             _settings.Issuer,
            audience:           _settings.Audience,
            claims:             claims,
            expires:            expiry,
            signingCredentials: creds);

        return new TokenResponseDto
        {
            AccessToken  = new JwtSecurityTokenHandler().WriteToken(token),
            RefreshToken = GenerateRefreshToken(),
            ExpiresAt    = expiry,
            UserId       = user.Id,
            Email        = user.Email ?? "",
            Role         = role
        };
    }

    private static string GenerateRefreshToken()
    {
        var bytes = new byte[64];
        RandomNumberGenerator.Fill(bytes);
        return Convert.ToBase64String(bytes);
    }

    public ClaimsPrincipal? ValidateToken(string token)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.SecretKey));
        try
        {
            var principal = new JwtSecurityTokenHandler().ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey         = key,
                ValidateIssuer           = true,
                ValidIssuer              = _settings.Issuer,
                ValidateAudience         = true,
                ValidAudience            = _settings.Audience,
                ValidateLifetime         = false
            }, out _);
            return principal;
        }
        catch { return null; }
    }
}
