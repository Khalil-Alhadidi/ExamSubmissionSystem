using Microsoft.EntityFrameworkCore;
using NotificationService.Infrastructure.Persistence;

namespace NotificationService.API.Endpoints;

public static class NotificationServiceEndpoints
{
    public static IEndpointRouteBuilder MapNotificationsEndpoints(this IEndpointRouteBuilder app)
    {

        app.MapGet("/logs", async (NotificationDbContext db) =>
        {
            var logs = await db.NotificationLogs.ToListAsync();
            return Results.Ok(logs);
        });

        return app;
    }

}
