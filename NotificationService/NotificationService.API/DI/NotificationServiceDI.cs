using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using NotificationService.Application.Interfaces;
using NotificationService.Application.UseCases;
using NotificationService.Infrastructure.Consumers;
using NotificationService.Infrastructure.Persistence;
using NotificationService.Infrastructure.Repositories;

namespace NotificationService.API.DI;

public static class NotificationServiceDI
{
    public static IServiceCollection AddNotificationServiceDI(this IServiceCollection services, IConfiguration configuration)
    {

        #region Swagger and OpenAPI
        services.AddOpenApi();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new() { Title = "Notification Service API", Version = "v1" });

        });

        #endregion

        #region Db Context
        
        services.AddDbContext<NotificationDbContext>(options =>
                        options.UseSqlServer(configuration.GetConnectionString("NotificationServiceDbConnection")));

        #endregion

        #region Repositories
        services.AddScoped<IAnswerSubmittedHandler, AnswerSubmittedHandler>();
        services.AddScoped<INotificationLogRepository, NotificationLogRepository>();

        #endregion

        #region RabbitMQ and MassTransit
        services.AddMassTransit(x =>
        {
            x.AddConsumer<AnswersSubmittedConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(configuration["RabbitMQ:Host"], "/", h =>
                {
                    h.Username(configuration["RabbitMQ:Username"]!);
                    h.Password(configuration["RabbitMQ:Password"]!);
                });
                // the queue
                cfg.ReceiveEndpoint("notification-service-answers-submitted", e =>
                {
                    e.ConfigureConsumer<AnswersSubmittedConsumer>(context);

                    e.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(5)));
                });
            });
        });

        #endregion


        return services;
    }

}
