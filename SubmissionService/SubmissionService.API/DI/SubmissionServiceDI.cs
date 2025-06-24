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
        #region Db Context
        services.AddDbContext<SubmissionDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("SubmissionServiceDbConnection"),
             sql => sql.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null)));
        #endregion

        #region Repositories, Services and Http Clients
        services.AddScoped<ISubmissionRepository, SubmissionRepository>();

        
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

        services.AddHttpClient("ExamServiceHealthCheck", client =>
        {
            client.BaseAddress = new Uri(configuration["Services:ExamServiceUrl"]!);
            client.Timeout = TimeSpan.FromSeconds(5); // short timeout for health check
        });

        #endregion

        #region Cached Services
        services.AddMemoryCache();
        services.AddScoped<ICachedExamServiceClient, CachedExamServiceClient>();
        #endregion

        #region RabbitMQ and MassTransit Configuration 
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
        #endregion

        return services;
    }
}
