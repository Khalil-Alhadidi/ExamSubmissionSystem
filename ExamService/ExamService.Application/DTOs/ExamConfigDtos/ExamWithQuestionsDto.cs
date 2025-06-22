using ExamService.Domain.Entities;

namespace ExamService.Application.DTOs.ExamConfigDtos;

public class ExamWithQuestionsDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public Guid SubjectId { get; set; }
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }
    public int TotalQuestions { get; set; }
    public List<QuestionsBank> Questions { get; set; } = new();
}
