using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Middleware;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        var requestId = Guid.NewGuid().ToString();
        context.Items["RequestId"] = requestId;

        var correlationId = context.Request.Headers.TryGetValue("X-Correlation-ID", out var cid)
            ? cid.ToString()
            : requestId;

        using (_logger.BeginScope(new Dictionary<string, object>
        {
            ["RequestId"] = requestId,
            ["CorrelationId"] = correlationId,
            ["Path"] = context.Request.Path,
            ["Method"] = context.Request.Method
        }))
        {
            _logger.LogInformation($"→ Handling request for {context.Request.Path}");

            var sw = Stopwatch.StartNew();
            await _next(context);
            sw.Stop();

            _logger.LogInformation("← Completed {StatusCode} in {ElapsedMs}ms",
                context.Response.StatusCode, sw.ElapsedMilliseconds);
        }
    }
}


