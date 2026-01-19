using System.Net;
using System.Text.Json;
using TrustGateAPI.Services.Interfaces;

namespace TrustGateAPI.Middleware;

public class ApiTokenAuthorizationMiddleware
{
    private readonly RequestDelegate _next;

    public ApiTokenAuthorizationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(
        HttpContext context,
        IApiTokenAccessService accessService)
    {
        var path = context.Request.Path.Value?.ToLower() ?? "";

        if (path.StartsWith("/swagger") ||
            path.StartsWith("/auth") ||
            path.StartsWith("/health"))
        {
            await _next(context);
            return;
        }

        if (!context.Request.Headers.TryGetValue("Authorization", out var header))
        {
            await Reject(context, "Missing Authorization header");
            return;
        }

        var token = header.ToString().Replace("Bearer ", "");

        // You must provide a companyId value here. 
        // If you have a way to get the companyId from the context, use it.
        // For now, using 0 as a placeholder. Replace with actual logic as needed.
        int companyId = 0;

        var hasAccess = await accessService.HasAccessAsync(
            token,
            context.Request.Method,
            context.Request.Path,
            companyId);

        if (!hasAccess)
        {
            await Reject(context, "API token has no access to this endpoint");
            return;
        }

        await _next(context);
    }

    private static async Task Reject(HttpContext context, string message)
    {
        context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
        context.Response.ContentType = "application/json";

        await context.Response.WriteAsync(JsonSerializer.Serialize(new
        {
            error = message
        }));
    }
}
