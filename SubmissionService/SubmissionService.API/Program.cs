using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Logging;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using Serilog.Enrichers.OpenTelemetry;
using Shared.Extensions;
using Shared.Middleware;
using SubmissionService.API.DI;
using SubmissionService.API.Endpoints;
using SubmissionService.Infrastructure.Persistence;


#region Logging and Telemetry

var builder = WebApplication.CreateBuilder(args);


builder.Host.UseSerilog((context, services, cfg) =>
    cfg.ReadFrom.Configuration(context.Configuration)
       .Enrich.FromLogContext()
       .Enrich.WithOpenTelemetryTraceId()
);


builder.Services.AddOpenTelemetry()
    .WithTracing(tracerProviderBuilder =>
    {
        tracerProviderBuilder
            .SetResourceBuilder(
                ResourceBuilder.CreateDefault()
                    .AddService("Submission Service"))
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddConsoleExporter(); // Export to console and can be changed later to Jaeger in the future
    });

#endregion

#region DI Service Registration
SubmissionServiceDI.AddSubmissionServiceDI(builder.Services, builder.Configuration);
SubmissionServiceInfraDI.AddSubmissionServiceDI(builder.Services, builder.Configuration);

#endregion

var app = builder.Build();

#region Swagger and Dev Endpoints
var enableSwagger = builder.Configuration.GetValue<bool>("Features:EnableSwagger");
var enableDevEndpoints = builder.Configuration.GetValue<bool>("Features:EnableDevEndpoints");

if (enableSwagger)
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (enableDevEndpoints)
{
    IdentityModelEventSource.ShowPII = true;
    app.MapDevTokenEndpoints();
}

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<SubmissionDbContext>();
    await db.Database.MigrateAsync();
}
#endregion

#region Middleware

app.UseCors("AllowAllOrigins");

app.UseMiddleware<RequestLoggingMiddleware>();

app.UseMiddleware<ErrorHandlingMiddleware>();


app.UseAuthentication();

app.UseClaimsGlobalValidation();

app.UseAuthorization();

#endregion

#region Endpoints

app.MapSubmissionEndpoints();

app.MapGet("/", () => "SubmissionService is running - current UTC Time is :" + DateTime.UtcNow);


app.MapGet("/ping-exam-service", async (IHttpClientFactory httpClientFactory, ILoggerFactory loggerFactory) =>
{
    var logger = loggerFactory.CreateLogger("PingExamService");
    try
    {
        logger.LogInformation("Pinging Exam Service at / endpoint");
        var client = httpClientFactory.CreateClient("ExamServiceHealthCheck");
        var result = await client.GetStringAsync("/");
        logger.LogInformation("Exam Service responded successfully");
        return Results.Ok(new { Status = "Reachable", Message = result });
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "ExamService unreachable");
        return Results.InternalServerError($"ExamService unreachable: {ex.Message}");
    }
}).WithDescription("Check connectivity with the Exam Service");

#endregion


app.Run();