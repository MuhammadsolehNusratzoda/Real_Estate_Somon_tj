using RealEstate.Domain.Common;

namespace RealEstate.Domain.Interfaces.Services;

public interface IAuthService
{
    Task<GenericResponse<object>> RegisterAsync(string email, string password, string firstName, string lastName, string role);
    Task<GenericResponse<object>> LoginAsync(string email, string password);
    Task<GenericResponse<object>> RefreshTokenAsync(string refreshToken);
    Task<GenericResponse<object>> LogoutAsync(string userId);
    Task<GenericResponse<object>> GetCurrentUserAsync(string userId);
}
