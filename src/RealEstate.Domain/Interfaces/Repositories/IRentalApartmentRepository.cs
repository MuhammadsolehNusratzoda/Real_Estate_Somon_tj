using RealEstate.Domain.Entities.Property;

namespace RealEstate.Domain.Interfaces.Repositories;

public interface IRentalApartmentRepository : IGenericRepository<RentalApartment>
{
    Task<IEnumerable<RentalApartment>> GetBySellerIdAsync(string sellerId);
    Task<int> MarkExpiredAsInactiveAsync(int expiryDays);
}
