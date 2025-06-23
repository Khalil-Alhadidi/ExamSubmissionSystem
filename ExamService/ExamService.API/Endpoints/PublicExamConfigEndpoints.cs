using ExamService.Application.UseCases.ExamConfigs.Read;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Shared.Contracts.ExamService;

namespace ExamService.API.Endpoints;

public static class PublicExamConfigEndpoints
{
    public static IEndpointRouteBuilder MapPublicExamConfigEndpoints(this IEndpointRouteBuilder app)
    {
        var publicExamGroup = app.MapGroup("/api/v1/exam-configs/public")
                         .WithTags("Public Exam Config API");

        //// API Key validation filter
        //// This should be used to protect public endpoints that require an API key for access, simple implementation
        //// given the time for this assesment
        publicExamGroup.AddEndpointFilter(async (context, next) =>
        {
            var httpContext = context.HttpContext;
            var expectedApiKey = CommunicationKey.ApiKey;
            if (!httpContext.Request.Headers.TryGetValue("X-Internal-Api-Key", out var providedKey) || providedKey != expectedApiKey)
            {
                return Results.Unauthorized();
            }
            return await next(context);
        });

        
        publicExamGroup.MapGet("/{id:guid}", async (
            Guid id,
            GetPublicExamConfigByIdHandler handler) =>
        {
            var result = await handler.HandleAsync(id);
            return result is null ? Results.NotFound() : Results.Ok(result);
        });

        return app;
    }
}
