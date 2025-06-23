using ExamService.Application.UseCases.ExamConfigs.Read;

namespace ExamService.API.Endpoints;

public static class PublicExamConfigEndpoints
{
    public static IEndpointRouteBuilder MapPublicExamConfigEndpoints(this IEndpointRouteBuilder app)
    {
        var publicExamGroup = app.MapGroup("/api/v1/exam-configs/public")
                         .WithTags("Public Exam Config API");

        // No auth required
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
