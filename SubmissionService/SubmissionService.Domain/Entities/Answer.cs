using Shared.Entities;

namespace SubmissionService.Domain.Entities;

public class Answer : AuditableEntity
{
    public Guid QuestionId { get; set; }
    public string? AnswerValue { get; set; } // Stores the answer for any question type
    public string QuestionType { get; set; } = string.Empty; // e.g., "TrueFalse", "Narrative", etc.
}