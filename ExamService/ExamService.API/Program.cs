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
                    .AddService("Exam Service"))
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
           .AddConsoleExporter(); // Export to console and can be changed later to Jaeger in the future
    });
#endregion

#region DI Service Registration 
ExamServiceDI.AddExamServiceDI(builder.Services, builder.Configuration);
ExamServiceSecurityDI.AddExamServiceInfraDI(builder.Services, builder.Configuration);
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

// Migrate and Seed Database
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ExamDbContext>();
    await db.Database.MigrateAsync();
    await ExamDbSeeder.SeedAsync(db);
}

#endregion

#region Middlewares
app.UseCors("AllowAllOrigins");

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseAuthentication();
app.UseClaimsGlobalValidation();
app.UseAuthorization();

#endregion

#region Endpoints

app.MapExamEndpoints();
app.MapSubjectsEndpoints();
app.MapQuestionBanksEndpoints();
app.MapPublicExamConfigEndpoints();

app.MapGet("/", () => "ExamService is running - current UTC Time is :"+DateTime.UtcNow);

#endregion

app.Run();
