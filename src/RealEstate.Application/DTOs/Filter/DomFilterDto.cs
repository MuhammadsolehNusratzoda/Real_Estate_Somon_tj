using RealEstate.Domain.Enums;

namespace RealEstate.Application.DTOs.Filter;

public class DomFilterDto
{
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public double? MinArea { get; set; }
    public double? MaxArea { get; set; }
    public int? MinRooms { get; set; }
    public int? MaxRooms { get; set; }
    public int? MinFloor { get; set; }
    public int? MaxFloor { get; set; }
    public string? Entrance { get; set; }
    public bool? HasBalcony { get; set; }
    public bool? HasElevator { get; set; }
    public string? Region { get; set; }
    public string? District { get; set; }
    public PropertyStatus? Status { get; set; }
    public string? SortBy { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
