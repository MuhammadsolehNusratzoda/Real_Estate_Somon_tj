namespace RealEstate.API.Extensions;

public static class AuthExtensions
{
    public static IServiceCollection AddAuthorizationPolicies(this IServiceCollection services)
    {
        services.AddAuthorization(opts =>
        {
            opts.AddPolicy("AdminOnly",  policy => policy.RequireRole("Admin"));
            opts.AddPolicy("SellerOnly", policy => policy.RequireRole("Seller", "Admin"));
            opts.AddPolicy("AnyUser",    policy => policy.RequireRole("Admin", "Seller", "Buyer"));
        });
        return services;
    }
}
