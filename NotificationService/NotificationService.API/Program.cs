
using Microsoft.IdentityModel.Logging;
using NotificationService.API.DI;
using NotificationService.API.Endpoints;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
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
                    .AddService("Notification Service"))
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddConsoleExporter(); // Export to console and can be changed later
    });

#endregion


NotificationServiceDI.AddNotificationServiceDI(builder.Services, builder.Configuration);


var app = builder.Build();

var enableSwagger = builder.Configuration.GetValue<bool>("Features:EnableSwagger");

if (enableSwagger)
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
    IdentityModelEventSource.ShowPII = true;
}


app.MapNotificationsEndpoints();

app.MapGet("/", () => "NotificationService is up & running - current UTC Time is :" + DateTime.UtcNow);


app.Run();


