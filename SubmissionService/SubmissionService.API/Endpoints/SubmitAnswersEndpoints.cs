using Azure.Core;
using Shared.Contracts.ExamService;
using SubmissionService.Application.DTOs;
using SubmissionService.Application.UseCases.SubmitExam;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SubmissionService.API.Endpoints;

public static  class SubmitAnswersEndpoints
{
    public static IEndpointRouteBuilder MapSubmissionEndpoints(this IEndpointRouteBuilder app)
    {

        var v1 = app.MapGroup("/api/v1/submit")
    .WithTags("Submit Answers API v1")
    .RequireAuthorization("StudentOnly");

        v1.MapPost("/submissions/{examId:guid}", async (
    Guid examId,
    SubmitExamRequest request,
    HttpContext httpContext,
    SubmitExamHandler handler,
    IExamServiceClient examClient) =>
        {

            // Extract StudentId (from token's "sub" claim)
            var userId = httpContext.User.FindFirst(ClaimTypes.Name)?.Value;

            if (!Guid.TryParse(userId, out var studentId))
                return Results.Unauthorized();

            // Get exam config from ExamService
            var config = await examClient.GetExamConfigAsync(examId);
            if (config is null)
                return Results.NotFound("Exam config not found");

            var now = DateTime.UtcNow;
            if (now < config.OpenTimeUtc || now > config.CloseTimeUtc)
                return Results.BadRequest("Exam is not currently open for submissions.");

            try
            {
                // Save submission with validation against config questions
                var submissionId = await handler.HandleAsync(request, studentId, config.Questions);
                return Results.Ok(new { SubmissionId = submissionId });
            }
            catch (InvalidOperationException ex)
            {
                return Results.BadRequest(ex.Message);
            }
        });

        return app;
    }
}
