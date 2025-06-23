using ExamService.API;
using ExamService.API.Endpoints;
using ExamService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using OpenTelemetry.Metrics;
using Serilog;

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
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(); 
}

//app.UseHttpsRedirection();


app.UseMiddleware<ErrorHandlingMiddleware>();


using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ExamDbContext>();
    await db.Database.MigrateAsync(); 
    await ExamDbSeeder.SeedAsync(db);
}

app.UseCors("AllowAllOrigins");
app.MapExamEndpoints();
app.MapSubjectsEndpoints();
app.MapQuestionBanksEndpoints();

app.MapGet("/", () => "ExamService is running");


app.Run();
 