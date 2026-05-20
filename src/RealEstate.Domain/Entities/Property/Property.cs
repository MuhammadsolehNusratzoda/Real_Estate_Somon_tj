using RealEstate.Domain.Entities.User;
using RealEstate.Domain.Enums;

namespace RealEstate.Domain.Entities.Property;

public abstract class Property
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Address { get; set; } = string.Empty;
    public string District { get; set; } = string.Empty;
    public string Region { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public double Area { get; set; }
    public int Rooms { get; set; }
    public bool HasBathroom { get; set; }
    public bool HasToilet { get; set; }
    public List<string> ImageUrls { get; set; } = new();
    public PropertyStatus Status { get; set; } = PropertyStatus.Available;
    public PropertyType Type { get; set; }
    public ListingType ListingType { get; set; }
    public string SellerId { get; set; } = string.Empty;
    public AppUser? Seller { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
