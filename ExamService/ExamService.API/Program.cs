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
using ExamService.API.DI;
using Serilog.Enrichers.OpenTelemetry;

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
                    .AddService("Exam Service"))
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddConsoleExporter(); // Export to console and can be changed later
    });

#endregion


ExamServiceDI.AddExamServiceDI(builder.Services, builder.Configuration);

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


//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//    app.MapOpenApi();
//    IdentityModelEventSource.ShowPII = true;
//    app.MapDevTokenEndpoints();
//}

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
