namespace SubmissionService.Application.DTOs;

public class AnswerDto
{
    public Guid QuestionId { get; set; }
    public string? SelectedOption { get; set; } // for TrueFalse
    public string? NarrativeAnswerText { get; set; } // for Narrative
}
