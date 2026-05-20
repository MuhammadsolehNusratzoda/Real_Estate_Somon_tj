using RealEstate.Domain.Enums;

namespace RealEstate.Domain.Entities.Property;

public class Havli : Property
{
    public double LandArea { get; set; }
    public bool HasGarage { get; set; }
    public bool HasPool { get; set; }
    public bool HasBasement { get; set; }
    public string FenceType { get; set; } = string.Empty;

    public Havli() { Type = PropertyType.Havli; ListingType = ListingType.ForSale; }
}
