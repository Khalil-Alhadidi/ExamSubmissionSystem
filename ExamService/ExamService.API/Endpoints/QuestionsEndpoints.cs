using ExamService.Application.DTOs.Questions;
using ExamService.Application.UseCases.Questions.Create;
using ExamService.Application.UseCases.Questions.Delete;
using ExamService.Application.UseCases.Questions.Read;
using ExamService.Application.UseCases.Questions.Update;

namespace ExamService.API.Endpoints;

public static class QuestionsEndpoints
{
    public static IEndpointRouteBuilder MapQuestionBanksEndpoints(this IEndpointRouteBuilder app)
    {
        var v1 = app.MapGroup("/api/v1/questions")
            .WithTags("Questions Bank API v1")
            .RequireAuthorization("AdminOnly"); 

        v1.MapGet("/", async (GetQuestionsHandler handler) =>
        {
            var result = await handler.HandleAsync();
            return Results.Ok(result);
        })
            .WithSummary("List all questions")
            .WithDescription("Returns all questions available in the system.");

        v1.MapGet("/{id:guid}", async (Guid id, GetQuestionByIdHandler handler) =>
        {
            var result = await handler.HandleAsync(id);
            return result is null ? Results.NotFound() : Results.Ok(result);
        })
            .WithSummary("Get question by ID")
            .WithDescription("Retrieves a question by its unique identifier. Returns 404 if not found.");

        v1.MapGet("/by-subject/{subjectId:guid}", async (Guid subjectId, GetQuestionsBySubjectHandler handler) =>
        {
            var result = await handler.HandleAsync(subjectId);
            return Results.Ok(result);
        })
            .WithSummary("List questions by subject")
            .WithDescription("Returns all questions associated with a specific subject.");

        v1.MapPost("/", async (CreateQuestionDto dto, CreateQuestionHandler handler) =>
        {
            await handler.HandleAsync(dto);
            return Results.Created("/api/questions", dto);
        })
            .WithSummary("Create a new question")
            .WithDescription("Creates a new question with the provided details.");

        v1.MapPut("/{id:guid}", async (Guid id, UpdateQuestionDto dto, UpdateQuestionHandler handler) =>
        {
            var updated = await handler.HandleAsync(id, dto);
            return updated ? Results.NoContent() : Results.NotFound();
        })
            .WithSummary("Update an existing question")
            .WithDescription("Updates the details of an existing question by its unique identifier.");

        v1.MapDelete("/{id}", async (Guid id, SoftDeleteQuestionHandler handler) =>
        {
            await handler.HandleAsync(id);
            return Results.NoContent();
        })
            .WithSummary("Delete a question")
            .WithDescription("Deletes a question by its unique identifier.");

        return app;
    }
}
