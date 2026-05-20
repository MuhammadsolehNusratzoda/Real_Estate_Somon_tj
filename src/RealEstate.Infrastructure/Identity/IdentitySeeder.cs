using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RealEstate.Domain.Entities.User;

namespace RealEstate.Infrastructure.Identity;

public static class IdentitySeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = services.GetRequiredService<UserManager<AppUser>>();
        var logger      = services.GetRequiredService<ILogger<AppUser>>();

        string[] roles = { "Admin", "Seller", "Buyer" };
        foreach (var role in roles)
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));

        const string adminEmail    = "admin@realestate.tj";
        const string adminPassword = "Admin@123456";

        if (await userManager.FindByEmailAsync(adminEmail) is null)
        {
            var admin = new AppUser
            {
                UserName  = adminEmail,
                Email     = adminEmail,
                FirstName = "System",
                LastName  = "Admin"
            };
            var result = await userManager.CreateAsync(admin, adminPassword);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(admin, "Admin");
                logger.LogInformation("Default admin created: {Email}", adminEmail);
            }
        }
    }
}
