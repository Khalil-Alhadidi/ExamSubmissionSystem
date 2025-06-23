using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace Shared.Middleware;

public class ClaimsValidationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ClaimsValidationMiddleware> _logger;

    public ClaimsValidationMiddleware(RequestDelegate next, ILogger<ClaimsValidationMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var user = context.User;

       

        if (user.Identity?.IsAuthenticated == true)
        {
            // Accept either "sub" or ClaimTypes.NameIdentifier as user id
            var hasUserId = user.HasClaim(c => c.Type == "sub") || user.HasClaim(c => c.Type == ClaimTypes.NameIdentifier);
            var hasRole = user.HasClaim(c => c.Type == "role") || user.HasClaim(c => c.Type == ClaimTypes.Role);

            if (!hasUserId || !hasRole)
            {
                _logger.LogWarning("Token missing required claims. Path: {Path}", context.Request.Path);

                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("Token is missing required claims.");
                return;
            }
        }

        await _next(context);
    }
}