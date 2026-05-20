using System.Net;
using System.Text.Json;
using RealEstate.Domain.Common;

namespace RealEstate.API.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next; _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        context.Response.ContentType = "application/json";
        var response = ex switch
        {
            UnauthorizedAccessException => new { isSuccess = false, message = ex.Message, statusCode = 401 },
            KeyNotFoundException        => new { isSuccess = false, message = ex.Message, statusCode = 404 },
            ArgumentException           => new { isSuccess = false, message = ex.Message, statusCode = 422 },
            _                           => new { isSuccess = false, message = "An internal server error occurred", statusCode = 500 }
        };
        context.Response.StatusCode = response.statusCode;
        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}

public static class ExceptionHandlingMiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionHandlingMiddleware(this IApplicationBuilder app)
        => app.UseMiddleware<ExceptionHandlingMiddleware>();
}
