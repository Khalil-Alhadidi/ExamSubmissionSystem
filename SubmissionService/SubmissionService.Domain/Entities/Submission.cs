using Shared.Entities;

namespace SubmissionService.Domain.Entities;

public class Submission : AuditableEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid StudentId { get; set; }
    public Guid ExamId { get; set; }
    public DateTimeOffset SubmittedAtUtc { get; set; } = DateTimeOffset.UtcNow;
    public List<Answer> Answers { get; set; } = new();
}
