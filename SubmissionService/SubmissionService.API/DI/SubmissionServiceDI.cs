using FluentValidation;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Polly;
using Shared.Contracts.ExamService;
using Shared.Interfaces;
using Shared.Services;
using SubmissionService.Application.Interfaces;
using SubmissionService.Application.UseCases.SubmitExam;
using SubmissionService.Infrastructure.Persistence;
using SubmissionService.Infrastructure.Repositories;
using SubmissionService.Infrastructure.Services;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;

namespace SubmissionService.API.DI;

public static class SubmissionServiceDI
{
    public static IServiceCollection AddSubmissionServiceDI(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOpenApi();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new() { Title = "SubmissionService API", Version = "v1" });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Enter your JWT like this: eyJ..."
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
        });




        services.AddCors(options =>
        {
            options.AddPolicy("AllowAllOrigins",
                builder => builder.AllowAnyOrigin()
                                  .AllowAnyMethod()
                                  .AllowAnyHeader());
        });

        // Database Context
        services.AddDbContext<SubmissionDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("SubmissionServiceDbConnection"),
             sql => sql.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null)));

        // To mimic the behavior of a real user, in real world this should be coming from UserService
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserService, CurrentUserService>();


        
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(options =>
            {
                //options.Authority = "https://your-auth-provider"; // external or future UserService


                options.TokenValidationParameters = new TokenValidationParameters
                {
                    // future UserService
                    ValidateIssuer = false,
                    ValidateAudience = false,

                    ValidateLifetime = true,

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes("This will be 256 bit secret and it should be 512 if I go with HmacSha512, it must be read from a secure location, this !s for Testing only0_0") // 
            ),

                    NameClaimType = ClaimTypes.NameIdentifier,//"sub"
                    RoleClaimType = ClaimTypes.Role //"role" 
                };
            });


        services.AddAuthorization(options =>
        {
            options.AddPolicy("StudentOnly", policy =>
                policy.RequireRole("user"));
        });


        // repositories
        services.AddScoped<ISubmissionRepository, SubmissionRepository>();

        //Handlers
        services.AddScoped<SubmitExamHandler>();

       
        services.AddHttpClient<IExamServiceClient, ExamServiceClient>(client =>
        {
            client.BaseAddress = new Uri(configuration["Services:ExamServiceUrl"]!);
        })
        .AddTransientHttpErrorPolicy(policy =>
         policy.WaitAndRetryAsync(
        retryCount: 3,
        sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), // 2s, 4s, 8s
        onRetry: (outcome, timespan, retryAttempt, context) =>
        {
            Console.WriteLine($"Retry {retryAttempt} due to {outcome.Exception?.Message}");
        }));

        //caching
        services.AddScoped<ICachedExamServiceClient, CachedExamServiceClient>();

        services.AddHttpClient("ExamServiceHealthCheck", client =>
        {
            client.BaseAddress = new Uri(configuration["Services:ExamServiceUrl"]!);
            client.Timeout = TimeSpan.FromSeconds(5); // short timeout for health check
        });


        // Application Services
        services.Configure<JsonOptions>(options =>
        {
            options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });

        services.AddMassTransit(x =>
        {
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(configuration["RabbitMQ:Host"], "/", h =>
                {
                    h.Username("rabbitmq");// this should be read from secure location
                    h.Password("Admin@1234");
                });
            });
        });

        services.AddScoped<IEventPublisher, EventPublisher>();


        services.AddMemoryCache();

        return services;
    }
}
