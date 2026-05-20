using System.Diagnostics;
using System.Security.Claims;

namespace RealEstate.API.Middleware;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next; _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var sw     = Stopwatch.StartNew();
        var userId = context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "anonymous";

        await _next(context);

        sw.Stop();
        _logger.LogInformation(
            "[{Method}] {Path}{Query} | Status: {StatusCode} | Duration: {Ms}ms | User: {UserId}",
            context.Request.Method,
            context.Request.Path,
            context.Request.QueryString,
            context.Response.StatusCode,
            sw.ElapsedMilliseconds,
            userId);
    }
}

public static class RequestLoggingMiddlewareExtensions
{
    public static IApplicationBuilder UseRequestLoggingMiddleware(this IApplicationBuilder app)
        => app.UseMiddleware<RequestLoggingMiddleware>();
}
