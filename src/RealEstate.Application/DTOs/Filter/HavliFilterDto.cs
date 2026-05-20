using RealEstate.Domain.Enums;

namespace RealEstate.Application.DTOs.Filter;

public class HavliFilterDto
{
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public double? MinArea { get; set; }
    public double? MaxArea { get; set; }
    public double? MinLandArea { get; set; }
    public double? MaxLandArea { get; set; }
    public int? MinRooms { get; set; }
    public int? MaxRooms { get; set; }
    public bool? HasGarage { get; set; }
    public bool? HasPool { get; set; }
    public string? Region { get; set; }
    public string? District { get; set; }
    public PropertyStatus? Status { get; set; }
    public string? SortBy { get; set; } // price_asc | price_desc | newest | oldest
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
