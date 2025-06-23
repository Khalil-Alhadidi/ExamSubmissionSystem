using ExamService.Infrastructure.Middleware;
using Microsoft.AspNetCore.Builder;

namespace ExamService.Infrastructure.Extensions;

public static class ClaimsValidationMiddlewareExtensions
{
    public static IApplicationBuilder UseClaimsValidation(this IApplicationBuilder app)
    {
        return app.UseMiddleware<ClaimsValidationMiddleware>();
    }
}
