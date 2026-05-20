using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RealEstate.Domain.Entities.Property;
using RealEstate.Domain.Entities.User;

namespace RealEstate.Infrastructure.Data;

public class AppDbContext : IdentityDbContext<AppUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Havli> Havlis { get; set; }
    public DbSet<DomApartment> DomApartments { get; set; }
    public DbSet<RentalApartment> RentalApartments { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Havli>(e =>
        {
            e.ToTable("Havlis");
            e.HasKey(x => x.Id);
            e.Property(x => x.Price).HasPrecision(18, 2);
            e.Property(x => x.ImageUrls).HasConversion(
                v => string.Join(',', v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList());
        });

        builder.Entity<DomApartment>(e =>
        {
            e.ToTable("DomApartments");
            e.HasKey(x => x.Id);
            e.Property(x => x.Price).HasPrecision(18, 2);
            e.Property(x => x.ImageUrls).HasConversion(
                v => string.Join(',', v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList());
        });

        builder.Entity<RentalApartment>(e =>
        {
            e.ToTable("RentalApartments");
            e.HasKey(x => x.Id);
            e.Property(x => x.Price).HasPrecision(18, 2);
            e.Property(x => x.MonthlyRent).HasPrecision(18, 2);
            e.Property(x => x.ImageUrls).HasConversion(
                v => string.Join(',', v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList());
        });
    }
}
