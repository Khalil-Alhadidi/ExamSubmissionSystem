using ExamService.Application.DTOs.ExamConfigDtos;
using ExamService.Application.UseCases.ExamConfigs.Create;
using ExamService.Application.UseCases.ExamConfigs.Delete;
using ExamService.Application.UseCases.ExamConfigs.Read;
using ExamService.Application.UseCases.ExamConfigs.Update;
using FluentValidation;

namespace ExamService.API.Endpoints;

public static class ExamEndpoints
{
    public static IEndpointRouteBuilder MapExamEndpoints(this IEndpointRouteBuilder app)
    {
        var v1 = app.MapGroup("/api/v1/exams")
            .WithTags("Exam Configuration API v1")
            .RequireAuthorization("AdminOnly"); 

        v1.MapGet("/", async (GetExamConfigsHandler handler) =>
        {
            var result = await handler.HandleAsync();
            return Results.Ok(result);
        })
            .WithSummary("List all exams")
            .WithDescription("Returns all exam configurations available in the system.");

        v1.MapGet("/{id:guid}", async (Guid id, GetExamConfigByIdHandler handler) =>
        {
            var result = await handler.HandleAsync(id);
            return result is null ? Results.NotFound() : Results.Ok(result);
        })
            .WithSummary("Get exam by ID")
            .WithDescription("Retrieves an exam configuration by its unique identifier. Returns 404 if not found.");

        v1.MapGet("/{id:guid}/with-questions", async (Guid id, GetExamWithQuestionsHandler handler) =>
        {
            var result = await handler.HandleAsync(id);
            return result is null ? Results.NotFound() : Results.Ok(result);
        })
            .WithSummary("Get exam with questions")
            .WithDescription("Retrieves an exam configuration along with its associated questions by exam ID. Returns 404 if not found.");

        v1.MapPost("/", async (
            CreateExamConfigDto dto,
            IValidator<CreateExamConfigDto> validator,
            CreateExamConfigHandler handler) =>
        {
            var validationResult = await validator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => new { e.PropertyName, e.ErrorMessage });
                return Results.BadRequest(errors);
            }

            await handler.HandleAsync(dto);
            return Results.Created("/api/exams", dto);
        })
            .WithSummary("Create a new exam")
            .WithDescription("Creates a new exam configuration with the provided details. Returns validation errors if the input is invalid.");

        v1.MapPut("/{id:guid}", async (Guid id, UpdateExamConfigDto dto, UpdateExamConfigHandler handler) =>
        {
            var updated = await handler.HandleAsync(id, dto);
            return updated ? Results.NoContent() : Results.NotFound();
        })
            .WithSummary("Update an existing exam")
            .WithDescription("Updates the details of an existing exam configuration by its unique identifier. Returns 404 if not found.");

        v1.MapDelete("/{id}", async (Guid id, DeleteExamConfigHandler handler) =>
        {
            await handler.HandleAsync(id);
            return Results.NoContent();
        })
            .WithSummary("Delete an exam")
            .WithDescription("Deletes an exam configuration by its unique identifier.");

        return app;
    }
}

