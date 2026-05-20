using Microsoft.EntityFrameworkCore;
using RealEstate.Domain.Entities.Property;
using RealEstate.Domain.Enums;
using RealEstate.Domain.Interfaces.Repositories;
using RealEstate.Infrastructure.Data;

namespace RealEstate.Infrastructure.Repositories;

public class RentalApartmentRepository : GenericRepository<RentalApartment>, IRentalApartmentRepository
{
    public RentalApartmentRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<RentalApartment>> GetBySellerIdAsync(string sellerId)
        => await _dbSet.Where(x => x.SellerId == sellerId).ToListAsync();

    public async Task<int> MarkExpiredAsInactiveAsync(int expiryDays)
    {
        var cutoff = DateTime.UtcNow.AddDays(-expiryDays);
        var expired = await _dbSet
            .Where(x => x.Status == PropertyStatus.Available && x.CreatedAt < cutoff)
            .ToListAsync();
        foreach (var e in expired) { e.Status = PropertyStatus.Inactive; e.UpdatedAt = DateTime.UtcNow; }
        await _context.SaveChangesAsync();
        return expired.Count;
    }
}
