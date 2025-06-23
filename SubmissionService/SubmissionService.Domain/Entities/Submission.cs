using Shared.Entities;

namespace SubmissionService.Domain.Entities;

public class Submission : AuditableEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid StudentId { get; set; }
    public Guid ExamId { get; set; }
    public DateTime SubmittedAtUtc { get; set; } = DateTime.UtcNow;
    public List<Answer> Answers { get; set; } = new();
}
