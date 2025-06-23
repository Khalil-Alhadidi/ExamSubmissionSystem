using Microsoft.AspNetCore.Builder;
using Shared.Middleware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Extensions;

public static class ClaimsValidationGlobalMiddlewareExtensions
{
    public static IApplicationBuilder UseClaimsGlobalValidation(this IApplicationBuilder app)
    {
        return app.UseMiddleware<ClaimsValidationMiddleware>();
    }
}
