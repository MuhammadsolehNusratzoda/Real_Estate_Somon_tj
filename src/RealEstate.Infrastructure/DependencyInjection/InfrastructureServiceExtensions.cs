using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using RealEstate.Application.DTOs.Auth;
using RealEstate.Application.Services;
using RealEstate.Domain.Entities.User;
using RealEstate.Domain.Interfaces.Repositories;
using RealEstate.Domain.Interfaces.Services;
using RealEstate.Infrastructure.BackgroundServices;
using RealEstate.Infrastructure.Cache;
using RealEstate.Infrastructure.Data;
using RealEstate.Infrastructure.JWT;
using RealEstate.Infrastructure.Repositories;
using System.Text;

namespace RealEstate.Infrastructure.DependencyInjection;

public static class InfrastructureServiceExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        // DbContext
        services.AddDbContext<AppDbContext>(opts =>
            opts.UseNpgsql(config.GetConnectionString("DefaultConnection")));

        // Identity
        services.AddIdentity<AppUser, IdentityRole>(opts =>
        {
            opts.Password.RequireDigit           = true;
            opts.Password.RequiredLength         = 8;
            opts.Password.RequireNonAlphanumeric  = true;
            opts.Password.RequireUppercase        = true;
        })
        .AddEntityFrameworkStores<AppDbContext>()
        .AddDefaultTokenProviders();

        // JWT
        var jwtSection = config.GetSection("JwtSettings");
        services.Configure<JwtSettings>(jwtSection);
        var jwtSettings = jwtSection.Get<JwtSettings>()!;

        services.AddAuthentication(opts =>
        {
            opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opts.DefaultChallengeScheme    = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(opts =>
        {
            opts.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey         = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
                ValidateIssuer           = true,
                ValidIssuer              = jwtSettings.Issuer,
                ValidateAudience         = true,
                ValidAudience            = jwtSettings.Audience,
                ValidateLifetime         = true,
                ClockSkew                = TimeSpan.Zero
            };
        });

        // Repositories
        services.AddScoped<IHavliRepository,           HavliRepository>();
        services.AddScoped<IDomApartmentRepository,    DomApartmentRepository>();
        services.AddScoped<IRentalApartmentRepository, RentalApartmentRepository>();

        // JWT Generator (used as Func in AuthService)
        services.AddScoped<JwtTokenGenerator>();
        services.AddScoped<Func<AppUser, Task<TokenResponseDto>>>(sp =>
        {
            var gen = sp.GetRequiredService<JwtTokenGenerator>();
            return gen.GenerateTokenAsync;
        });

        // Services
        services.AddScoped<IHavliService,           HavliService>();
        services.AddScoped<IDomApartmentService,    DomApartmentService>();
        services.AddScoped<IRentalApartmentService, RentalApartmentService>();
        services.AddScoped<IAuthService,            AuthService>();
        services.AddScoped<IAdminService,           AdminService>();

        // Cache
        services.AddMemoryCache();
        services.AddSingleton<CacheService>();

        // Background Service
        services.AddHostedService<PropertyStatusUpdaterService>();

        return services;
    }
}
