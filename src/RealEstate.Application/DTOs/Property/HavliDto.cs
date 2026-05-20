namespace RealEstate.Application.DTOs.Property;

public class HavliDto : PropertyBaseDto
{
    public double LandArea { get; set; }
    public bool HasGarage { get; set; }
    public bool HasPool { get; set; }
    public bool HasBasement { get; set; }
    public string FenceType { get; set; } = string.Empty;
}
