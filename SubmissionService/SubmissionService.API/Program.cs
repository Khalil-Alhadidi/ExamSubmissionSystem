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

Log.Logger = new LoggerConfiguration()
    .Enrich.WithOpenTelemetryTraceId()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();




var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

builder.Services.AddOpenTelemetry()
    .WithTracing(tracerProviderBuilder =>
    {
        tracerProviderBuilder
            .SetResourceBuilder(
                ResourceBuilder.CreateDefault()
                    .AddService("Submission Service"))
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddConsoleExporter(); // Export to console for now and can be changed later
    });

#endregion

SubmissionServiceDI.AddSubmissionServiceDI(builder.Services, builder.Configuration);


var app = builder.Build();

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

app.UseCors("AllowAllOrigins");

app.UseMiddleware<ErrorHandlingMiddleware>();


app.UseAuthentication();

app.UseClaimsGlobalValidation();

app.UseAuthorization();


app.MapSubmissionEndpoints();

app.MapGet("/", () => "SubmissionService is running - current UTC Time is :" + DateTime.UtcNow);


app.MapGet("/ping-exam", async (IHttpClientFactory httpClientFactory) =>
{
    try
    {
        var client = httpClientFactory.CreateClient("ExamServiceHealthCheck");
        var result = await client.GetStringAsync("/");
        return Results.Ok(new { Status = "Reachable", Message = result });
    }
    catch (Exception ex)
    {
        return Results.Problem($"ExamService unreachable: {ex.Message}");
    }
});


app.Run();

 