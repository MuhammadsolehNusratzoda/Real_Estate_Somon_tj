using RealEstate.Domain.Common;

namespace RealEstate.Domain.Interfaces.Services;

public interface IAdminService
{
    Task<GenericResponse<object>> GetDashboardAsync();
    Task<GenericResponse<object>> GetAllPropertiesAsync();
    Task<GenericResponse<object>> ChangePropertyStatusAsync(Guid propertyId, int newStatus, string propertyType);
    Task<GenericResponse<object>> ForceDeletePropertyAsync(Guid propertyId, string propertyType);
    Task<GenericResponse<object>> GetAllUsersAsync();
    Task<GenericResponse<object>> BlockUserAsync(string userId);
    Task<GenericResponse<object>> DeleteUserAsync(string userId);
}
