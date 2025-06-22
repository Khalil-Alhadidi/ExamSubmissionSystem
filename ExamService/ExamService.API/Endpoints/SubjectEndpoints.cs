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
        app.MapGet("/api/subjects", async (GetSubjectsHandler handler) =>
        {
            return Results.Ok(await handler.HandleAsync());
        });

        app.MapGet("/api/subjects/{id}", async (Guid id, GetSubjectByIdHandler handler) =>
        {
            var result = await handler.HandleAsync(id);
            return result is null ? Results.NotFound() : Results.Ok(result);
        });

        app.MapPost("/api/subjects", async (CreateSubjectDto dto, CreateSubjectHandler handler) =>
        {
            await handler.HandleAsync(dto);
            return Results.Created($"/api/subjects", dto);
        });

        app.MapPut("/api/subjects/{id}", async (Guid id, UpdateSubjectDto dto, UpdateSubjectHandler handler) =>
        {
            await handler.HandleAsync(id, dto);
            return Results.NoContent();
        });

        app.MapDelete("/api/subjects/{id}", async (Guid id, DeleteSubjectHandler handler) =>
        {
            await handler.HandleAsync(id);
            return Results.NoContent();
        });


        return app;
    }
}
