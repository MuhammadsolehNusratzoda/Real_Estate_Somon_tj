using RealEstate.API.Extensions;
using RealEstate.API.Middleware;
using RealEstate.Application.Mappings;
using RealEstate.Application.Validators;
using RealEstate.Infrastructure.DependencyInjection;
using RealEstate.Infrastructure.Identity;
using FluentValidation;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();

// Services
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

builder.Services.AddValidatorsFromAssemblyContaining<HavliCreateValidator>();

builder.Services.AddAuthorizationPolicies();

builder.Services.AddCorsPolicy();

builder.Services.AddSwaggerWithJwt();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Seed roles and default admin
using (var scope = app.Services.CreateScope())
{
    await IdentitySeeder.SeedAsync(scope.ServiceProvider);
}

// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Middleware
app.UseExceptionHandlingMiddleware();

app.UseRequestLoggingMiddleware();

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapGet("/health", () => Results.Ok(new
{
    status = "healthy",
    timestamp = DateTime.UtcNow
}));

app.Run();