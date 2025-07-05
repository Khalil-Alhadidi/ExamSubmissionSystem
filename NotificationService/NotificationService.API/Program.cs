
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Logging;
using NotificationService.API.DI;
using NotificationService.API.Endpoints;
using NotificationService.Infrastructure.Persistence;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using Serilog.Enrichers.OpenTelemetry;
using Shared.Middleware;


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
                    .AddService("Notification Service"))
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddConsoleExporter(); // Export to console and can be changed later to Jaeger in the future
    });

#endregion

#region DI Service Registration

NotificationServiceDI.AddNotificationServiceDI(builder.Services, builder.Configuration);

#endregion

var app = builder.Build();

#region Swagger and Dev Endpoints
var enableSwagger = builder.Configuration.GetValue<bool>("Features:EnableSwagger");

if (enableSwagger)
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
    IdentityModelEventSource.ShowPII = true;
}

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<NotificationDbContext>();
    await db.Database.MigrateAsync();
}

#endregion

#region Endpoints

app.UseMiddleware<RequestLoggingMiddleware>();
app.UseMiddleware<ErrorHandlingMiddleware>();


app.MapNotificationsEndpoints();

app.MapGet("/", () => "NotificationService is up & running - current UTC Time is :" + DateTime.UtcNow);

#endregion

app.Run();


