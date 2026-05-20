using Microsoft.AspNetCore.Identity;

namespace RealEstate.Domain.Entities.User;

public class AppUser : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName  { get; set; } = string.Empty;
    public bool IsBlocked   { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiry { get; set; }
}
