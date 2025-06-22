using ExamService.Application.DTOs.SubjectsDtos;
using ExamService.Application.UseCases.Subjects.Create;
using ExamService.Application.UseCases.Subjects.Delete;
using ExamService.Application.UseCases.Subjects.Read;
using ExamService.Application.UseCases.Subjects.Update;

namespace ExamService.API.Endpoints;

public static class SubjectEndpoints
{
    public static IEndpointRouteBuilder MapSubjectsEndpoints(this IEndpointRouteBuilder app)
    {

        var v1 = app.MapGroup("/api/v1/subjects")
        .WithTags("Exam API v1");

        v1.MapGet("/", async (GetSubjectsHandler handler) =>
        {
            return Results.Ok(await handler.HandleAsync());
        })
            .WithSummary("List all subjects")
            .WithDescription("Returns all available subjects configured in the system.");

        v1.MapGet("/{id}", async (Guid id, GetSubjectByIdHandler handler) =>
        {
            var result = await handler.HandleAsync(id);
            return result is null ? Results.NotFound() : Results.Ok(result);
        })
            .WithSummary("Get subject by ID")
            .WithDescription("Retrieves a subject by its unique identifier. Returns 404 if not found.");

        v1.MapPost("/", async (CreateSubjectDto dto, CreateSubjectHandler handler) =>
        {
            await handler.HandleAsync(dto);
            return Results.Created($"/api/subjects", dto);
        })
            .WithSummary("Create a new subject")
            .WithDescription("Creates a new subject with the provided details.");

        v1.MapPut("/{id}", async (Guid id, UpdateSubjectDto dto, UpdateSubjectHandler handler) =>
        {
            await handler.HandleAsync(id, dto);
            return Results.NoContent();
        })
            .WithSummary("Update an existing subject")
            .WithDescription("Updates the details of an existing subject by its unique identifier.");

        v1.MapDelete("/{id}", async (Guid id, DeleteSubjectHandler handler) =>
        {
            await handler.HandleAsync(id);
            return Results.NoContent();
        })
            .WithSummary("Delete a subject")
            .WithDescription("Deletes a subject by its unique identifier.");

        return app;
    }
}
