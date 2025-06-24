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
        services.AddOpenApi();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new() { Title = "Notification Service API", Version = "v1" });

        });


        services.AddDbContext<NotificationDbContext>(options =>
                        options.UseInMemoryDatabase("NotificationInMemoryDb"));



        services.AddScoped<IAnswerSubmittedHandler, AnswerSubmittedHandler>();
        services.AddScoped<INotificationLogRepository, NotificationLogRepository>();


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
                });
            });
        });


        return services;
    }

}
