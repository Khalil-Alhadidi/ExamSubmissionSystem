using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Logging;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using Shared.Extensions;
using Shared.Middleware;
using SubmissionService.API;
using SubmissionService.API.Endpoints;
using SubmissionService.Infrastructure.Persistence;


#region Logging and Telemetry
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day)
    .Enrich.FromLogContext()
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
            .AddConsoleExporter(); // Export to console for now (easy)
    });

#endregion

SubmissionServiceDI.AddSubmissionServiceDI(builder.Services, builder.Configuration);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
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

 