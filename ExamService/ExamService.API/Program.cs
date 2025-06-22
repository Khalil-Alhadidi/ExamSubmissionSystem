using ExamService.API;
using ExamService.API.Endpoints;
using ExamService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;


using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day)
    .Enrich.FromLogContext()
    .CreateLogger();



var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

builder.Services.AddOpenApi();


 

ExamServiceDI.AddExamServiceDI(builder.Services, builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(); // You can add options if needed
}

//app.UseHttpsRedirection();


app.UseMiddleware<ErrorHandlingMiddleware>();


using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ExamDbContext>();
    await db.Database.MigrateAsync(); 
    await ExamDbSeeder.SeedAsync(db);
}


app.MapExamEndpoints();
app.MapSubjectsEndpoints();
app.MapQuestionBanksEndpoints();

app.MapGet("/", () => "ExamService is running");


app.Run();
 