namespace ExamService.Domain.Entities;

public class ExamConfig
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public Guid SubjectId { get; set; }
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }
    public int TotalQuestions { get; set; }

    public List<Guid> QuestionIds { get; set; } = new();
}