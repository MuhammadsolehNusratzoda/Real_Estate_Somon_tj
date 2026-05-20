namespace RealEstate.Application.DTOs.Property;

public class HavliCreateDto
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
    public double LandArea { get; set; }
    public bool HasGarage { get; set; }
    public bool HasPool { get; set; }
    public bool HasBasement { get; set; }
    public string FenceType { get; set; } = string.Empty;
}
