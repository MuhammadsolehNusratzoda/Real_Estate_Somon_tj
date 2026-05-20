namespace RealEstate.Application.DTOs.Property;

public class RentalApartmentUpdateDto
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Address { get; set; }
    public string? District { get; set; }
    public string? Region { get; set; }
    public string? City { get; set; }
    public double? Area { get; set; }
    public int? Rooms { get; set; }
    public bool? HasBathroom { get; set; }
    public bool? HasToilet { get; set; }
    public List<string>? ImageUrls { get; set; }
    public int? Floor { get; set; }
    public int? TotalFloors { get; set; }
    public string? Entrance { get; set; }
    public bool? HasElevator { get; set; }
    public bool? HasBalcony { get; set; }
    public decimal? MonthlyRent { get; set; }
    public bool? UtilitiesIncluded { get; set; }
    public int? MinRentalMonths { get; set; }
}
