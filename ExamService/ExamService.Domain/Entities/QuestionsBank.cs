namespace ExamService.Domain.Entities;

public class QuestionsBank
{
    public Guid Id { get; set; }
    public Guid SubjectId { get; set; }
    public string Text { get; set; } = default!;
    public QuestionType Type { get; set; }
    public string? CorrectAnswer { get; set; }
}