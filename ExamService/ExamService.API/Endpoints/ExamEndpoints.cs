using ExamService.Application.UseCases.ExamConfig;

namespace ExamService.API.Endpoints;

public static class ExamEndpoints
{
    public static IEndpointRouteBuilder MapExamEndpoints(this IEndpointRouteBuilder app)
    {


        app.MapGet("/api/exams/{id}", async (Guid id, GetExamWithQuestionsHandler handler) =>
        {
            var result = await handler.HandleAsync(id);
            return result is null ? Results.NotFound() : Results.Ok(result);
        });

        return app;
    }
}

