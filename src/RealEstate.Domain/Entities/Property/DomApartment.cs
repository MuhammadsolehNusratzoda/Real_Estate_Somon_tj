using RealEstate.Domain.Enums;

namespace RealEstate.Domain.Entities.Property;

public class DomApartment : Property
{
    public int Floor { get; set; }
    public int TotalFloors { get; set; }
    public string Entrance { get; set; } = string.Empty;
    public bool HasElevator { get; set; }
    public bool HasBalcony { get; set; }

    public DomApartment() { Type = PropertyType.DomApartment; ListingType = ListingType.ForSale; }
}
