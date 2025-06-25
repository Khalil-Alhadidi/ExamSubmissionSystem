using Azure.Core;
using Shared.Contracts.ExamService;
using SubmissionService.Application.DTOs;
using SubmissionService.Application.UseCases.Get;
using SubmissionService.Application.UseCases.SubmitExam;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using static MassTransit.ValidationResultExtensions;

namespace SubmissionService.API.Endpoints;

public static class SubmitAnswersEndpoints
{
    public static IEndpointRouteBuilder MapSubmissionEndpoints(this IEndpointRouteBuilder app)
    {
        var v1 = app.MapGroup("/api/v1/submit")
            .WithTags("Submit Answers API v1")
            .RequireAuthorization("StudentOnly");


        v1.MapGet("/submissions", async (GetAllSubmissionsHandler handler) =>
        {
            var submissions = await handler.HandleAsync();
            if(submissions == null || !submissions.Any())
                return Results.NotFound("No submissions found.");
            return Results.Ok(submissions);
        })
        .WithSummary("Get all submissions")
        .WithDescription("Retrieves all exam submissions. Returns a list of all submissions in the system. Requires student authentication.");

        v1.MapGet("/submissions/by-student/{studentId:guid}", async (
            Guid studentId,
            GetSubmissionsByStudentHandler handler) =>
        {
            var submissions = await handler.HandleAsync(studentId);
            if(submissions == null || !submissions.Any())
                return Results.NotFound("No submissions found for the specified student ID.");
            return Results.Ok(submissions);
        })
        .WithSummary("Get submissions by student ID")
        .WithDescription("Retrieves all submissions for a specific student. Provide the studentId as a GUID in the route. Requires student authentication.");

        v1.MapGet("/submissions/by-exam/{examId:guid}", async (
             Guid examId,
         GetSubmissionsByExamHandler handler) =>
        {
            var submissions = await handler.HandleAsync(examId);
            if(submissions == null || !submissions.Any())
                return Results.NotFound("No submissions found for the specified exam ID.");
            return Results.Ok(submissions);
        })
        .WithSummary("Get submissions by exam ID")
        .WithDescription("Retrieves all submissions for a specific exam. Provide the examId as a GUID in the route. Requires student authentication.");

        v1.MapPost("/submissions/{examId:guid}", async (
            Guid examId,
            SubmitExamRequest request,
            HttpContext httpContext,
            SubmitExamHandler handler,
            ICachedExamServiceClient examClient) =>
        {
            var userId = httpContext.User.FindFirst(ClaimTypes.Name)?.Value;

            if (!Guid.TryParse(userId, out var studentId))
                return Results.Unauthorized();

            // Get exam config from ExamService
            var result = await examClient.GetExamConfigAsync(examId);

            switch (result.Status)
            {
                case ExamConfigFetchStatus.Unavailable:
                    return Results.Problem(
                        detail: "The ExamService is currently unavailable and the exam config is not cached.",
                        statusCode: 503,
                        title: "Exam Config Unavailable");
                case ExamConfigFetchStatus.NotFound:
                    return Results.NotFound("Exam config not found for the provided exam ID.");
                case ExamConfigFetchStatus.Ok:
                    break;
            }

            var now = DateTimeOffset.UtcNow;
            if (now < result.Config.OpenTimeUtc || now > result.Config.CloseTimeUtc)
                return Results.BadRequest("Exam is not currently open for submissions.");

            try
            {
                // Save submission with validation against config questions
                var submissionId = await handler.HandleAsync(request, studentId, examId, result.Config.Questions);
                return Results.Ok(new { SubmissionId = submissionId });
            }
            catch (InvalidOperationException ex)
            {
                return Results.BadRequest(ex.Message);
            }
        })
        .WithSummary("Submit answers for an exam")
        .WithDescription("Submits answers for a specific exam. Provide the examId as a GUID in the route and the answers in the request body. Validates exam availability and question configuration. Returns the submission ID on success. Requires student authentication.");

        return app;
    }
}
