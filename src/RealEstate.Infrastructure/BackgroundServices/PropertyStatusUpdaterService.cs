using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using RealEstate.Domain.Interfaces.Repositories;

namespace RealEstate.Infrastructure.BackgroundServices;

public class PropertyStatusUpdaterService : BackgroundService
{
    private readonly IServiceProvider _services;
    private readonly ILogger<PropertyStatusUpdaterService> _logger;
    private readonly int _intervalHours;
    private readonly int _expiryDays;

    public PropertyStatusUpdaterService(
        IServiceProvider services,
        ILogger<PropertyStatusUpdaterService> logger,
        IConfiguration config)
    {
        _services     = services;
        _logger       = logger;
        _intervalHours = config.GetValue<int>("BackgroundService:PropertyCheckIntervalHours", 24);
        _expiryDays   = config.GetValue<int>("BackgroundService:ListingExpiryDays", 30);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("PropertyStatusUpdaterService started. Interval: {Hours}h, Expiry: {Days}d",
            _intervalHours, _expiryDays);

        while (!stoppingToken.IsCancellationRequested)
        {
            await UpdateExpiredListingsAsync();
            await Task.Delay(TimeSpan.FromHours(_intervalHours), stoppingToken);
        }
    }

    private async Task UpdateExpiredListingsAsync()
    {
        try
        {
            using var scope = _services.CreateScope();
            var havliRepo  = scope.ServiceProvider.GetRequiredService<IHavliRepository>();
            var domRepo    = scope.ServiceProvider.GetRequiredService<IDomApartmentRepository>();
            var rentalRepo = scope.ServiceProvider.GetRequiredService<IRentalApartmentRepository>();

            var h = await havliRepo.MarkExpiredAsInactiveAsync(_expiryDays);
            var d = await domRepo.MarkExpiredAsInactiveAsync(_expiryDays);
            var r = await rentalRepo.MarkExpiredAsInactiveAsync(_expiryDays);

            _logger.LogInformation("Expired listings marked inactive — Havli: {H}, Dom: {D}, Rental: {R}", h, d, r);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating expired listings");
        }
    }
}
