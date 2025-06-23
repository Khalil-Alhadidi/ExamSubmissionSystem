using Shared.Entities;

namespace SubmissionService.Domain.Entities;

public class Answer : AuditableEntity
{
    public Guid QuestionId { get; set; }
    public string? SelectedOption { get; set; } // for TrueFalse
    public string? NarrativeAnswerText { get; set; } // for Narrative
    public string QuestionType { get; set; } = string.Empty; // "TrueFalse" or "Narrative"
}