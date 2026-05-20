using RealEstate.Domain.Enums;

namespace RealEstate.Domain.Entities.Property;

public class RentalApartment : Property
{
    public int Floor { get; set; }
    public int TotalFloors { get; set; }
    public string Entrance { get; set; } = string.Empty;
    public bool HasElevator { get; set; }
    public bool HasBalcony { get; set; }
    public decimal MonthlyRent { get; set; }
    public bool UtilitiesIncluded { get; set; }
    public int MinRentalMonths { get; set; } = 1;

    public RentalApartment() { Type = PropertyType.RentalApartment; ListingType = ListingType.ForRent; }
}
