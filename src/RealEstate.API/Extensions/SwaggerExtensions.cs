using Microsoft.OpenApi.Models;

namespace RealEstate.API.Extensions;

public static class SwaggerExtensions
{
    public static IServiceCollection AddSwaggerWithJwt(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title       = "RealEstate API",
                Version     = "v1",
                Description = "Somon.tj-style Real Estate Platform API — .NET 9 + PostgreSQL + Clean Architecture"
            });

            var securityScheme = new OpenApiSecurityScheme
            {
                Name         = "Authorization",
                Description  = "Enter: Bearer {token}",
                In           = ParameterLocation.Header,
                Type         = SecuritySchemeType.ApiKey,
                Scheme       = "Bearer",
                BearerFormat = "JWT"
            };
            c.AddSecurityDefinition("Bearer", securityScheme);
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } },
                    Array.Empty<string>()
                }
            });
        });

        return services;
    }

    public static IApplicationBuilder UseSwaggerUI(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "RealEstate API v1");
            c.RoutePrefix = "swagger";
        });
        return app;
    }
}
