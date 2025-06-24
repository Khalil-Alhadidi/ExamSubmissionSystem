namespace SubmissionService.Application.DTOs;

public class AnswerDto
{
    public Guid QuestionId { get; set; }
    public string? AnswerValue { get; set; } // Stores the answer for any question type
}
