using ExamService.Application.DTOs.Questions;
using ExamService.Application.UseCases.ExamConfig;
using ExamService.Application.UseCases.Questions.Create;
using ExamService.Application.UseCases.Questions.Delete;
using ExamService.Application.UseCases.Questions.Read;
using ExamService.Application.UseCases.Questions.Update;

namespace ExamService.API.Endpoints;

public static class QuestionsEndpoints
{
    public static IEndpointRouteBuilder MapQuestionBanksEndpoints(this IEndpointRouteBuilder app)
    {


        app.MapGet("/api/questions", async (GetQuestionsHandler handler) =>
        {
            var result = await handler.HandleAsync();
            return Results.Ok(result);
        });

        app.MapGet("/api/questions/{id}", async (Guid id, GetQuestionByIdHandler handler) =>
        {
            var result = await handler.HandleAsync(id);
            return result is null ? Results.NotFound() : Results.Ok(result);
        });

        app.MapGet("/api/questions/by-subject/{subjectId}", async (Guid subjectId, GetQuestionsBySubjectHandler handler) =>
        {
            var result = await handler.HandleAsync(subjectId);
            return Results.Ok(result);
        });

        app.MapPost("/api/questions", async (CreateQuestionDto dto, CreateQuestionHandler handler) =>
        {
            await handler.HandleAsync(dto);
            return Results.Created("/api/questions", dto);
        });

        app.MapPut("/api/questions/{id}", async (Guid id, UpdateQuestionDto dto, UpdateQuestionHandler handler) =>
        {
            var updated = await handler.HandleAsync(id, dto);
            return updated ? Results.NoContent() : Results.NotFound();
        });

        app.MapDelete("/api/questions/{id}", async (Guid id, DeleteQuestionHandler handler) =>
        {
            await handler.HandleAsync(id);
            return Results.NoContent();
        });

        return app;
    }
}
