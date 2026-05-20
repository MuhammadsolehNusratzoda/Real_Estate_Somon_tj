using RealEstate.Domain.Common;
using RealEstate.Domain.Entities.Property;

namespace RealEstate.Domain.Interfaces.Repositories;

public interface IHavliRepository : IGenericRepository<Havli>
{
    Task<IEnumerable<Havli>> GetBySellerIdAsync(string sellerId);
    Task<int> MarkExpiredAsInactiveAsync(int expiryDays);
}
