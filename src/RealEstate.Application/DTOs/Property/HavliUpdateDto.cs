namespace RealEstate.Application.DTOs.Property;

public class HavliUpdateDto
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public decimal? Price { get; set; }
    public string? Address { get; set; }
    public string? District { get; set; }
    public string? Region { get; set; }
    public string? City { get; set; }
    public double? Area { get; set; }
    public int? Rooms { get; set; }
    public bool? HasBathroom { get; set; }
    public bool? HasToilet { get; set; }
    public List<string>? ImageUrls { get; set; }
    public double? LandArea { get; set; }
    public bool? HasGarage { get; set; }
    public bool? HasPool { get; set; }
    public bool? HasBasement { get; set; }
    public string? FenceType { get; set; }
}
