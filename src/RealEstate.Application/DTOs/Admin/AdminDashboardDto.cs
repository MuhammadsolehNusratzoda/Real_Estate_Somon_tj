namespace RealEstate.Application.DTOs.Admin;

public class AdminDashboardDto
{
    public int TotalListings { get; set; }
    public int TotalHavli { get; set; }
    public int TotalDomApartments { get; set; }
    public int TotalRentalApartments { get; set; }
    public int TotalSold { get; set; }
    public int TotalRented { get; set; }
    public int TotalAvailable { get; set; }
    public int TotalInactive { get; set; }
    public int TotalUsers { get; set; }
    public int TotalSellers { get; set; }
    public int TotalBuyers { get; set; }
    public int BlockedUsers { get; set; }
}
