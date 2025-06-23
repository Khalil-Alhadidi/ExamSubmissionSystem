using ExamService.API;
using ExamService.API.Endpoints;
using ExamService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Logging;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using Shared.Middleware;
using Shared.Extensions;

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
                    .AddService("Exam Service"))
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddConsoleExporter(); // Export to console for now (easy)
    });

#endregion


ExamServiceDI.AddExamServiceDI(builder.Services, builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
    IdentityModelEventSource.ShowPII = true;
    app.MapDevTokenEndpoints();
}

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ExamDbContext>();
    await db.Database.MigrateAsync();
    await ExamDbSeeder.SeedAsync(db);
}

app.UseCors("AllowAllOrigins");

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseAuthentication();
app.UseClaimsGlobalValidation();
app.UseAuthorization();

app.MapExamEndpoints();
app.MapSubjectsEndpoints();
app.MapQuestionBanksEndpoints();
app.MapPublicExamConfigEndpoints();

app.MapGet("/", () => "ExamService is running - current UTC Time is :"+DateTime.UtcNow);

app.Run();
