
using Shared.Entities;
namespace ExamService.Domain.Entities;

public class Subject : AuditableEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
}
