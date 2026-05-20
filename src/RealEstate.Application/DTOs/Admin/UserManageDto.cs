namespace RealEstate.Application.DTOs.Admin;

public class UserManageDto
{
    public string Id { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public bool IsBlocked { get; set; }
    public DateTime CreatedAt { get; set; }
}
