namespace RealEstate.Application.DTOs.Property;

public class DomApartmentDto : PropertyBaseDto
{
    public int Floor { get; set; }
    public int TotalFloors { get; set; }
    public string Entrance { get; set; } = string.Empty;
    public bool HasElevator { get; set; }
    public bool HasBalcony { get; set; }
}
