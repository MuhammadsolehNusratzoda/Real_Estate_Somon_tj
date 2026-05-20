namespace RealEstate.Application.DTOs.Property;

public class DomApartmentCreateDto
{
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
    public int Floor { get; set; }
    public int TotalFloors { get; set; }
    public string Entrance { get; set; } = string.Empty;
    public bool HasElevator { get; set; }
    public bool HasBalcony { get; set; }
}
