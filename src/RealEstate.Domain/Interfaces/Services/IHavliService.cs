using RealEstate.Domain.Common;

namespace RealEstate.Domain.Interfaces.Services;

public interface IHavliService
{
    Task<GenericResponse<object>> GetAllAsync(object filter);
    Task<GenericResponse<object>> GetByIdAsync(Guid id);
    Task<GenericResponse<object>> CreateAsync(object dto, string sellerId);
    Task<GenericResponse<object>> UpdateAsync(Guid id, object dto, string sellerId, bool isAdmin);
    Task<GenericResponse<object>> DeleteAsync(Guid id, string sellerId, bool isAdmin);
    Task<GenericResponse<object>> GetMyListingsAsync(string sellerId);
}
