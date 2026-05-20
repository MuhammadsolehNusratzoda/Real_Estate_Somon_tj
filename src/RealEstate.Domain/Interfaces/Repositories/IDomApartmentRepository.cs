using RealEstate.Domain.Entities.Property;

namespace RealEstate.Domain.Interfaces.Repositories;

public interface IDomApartmentRepository : IGenericRepository<DomApartment>
{
    Task<IEnumerable<DomApartment>> GetBySellerIdAsync(string sellerId);
    Task<int> MarkExpiredAsInactiveAsync(int expiryDays);
}
