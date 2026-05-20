using RealEstate.Domain.Entities.Property;

namespace RealEstate.Domain.Interfaces.Repositories;

public interface IPropertyRepository : IGenericRepository<Property>
{
    Task<IEnumerable<Property>> GetBySellerIdAsync(string sellerId);
}
