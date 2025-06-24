using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Domain.Entities;

public class NotificationLog 
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid SubmissionId { get; set; }
    public Guid StudentId { get; set; }
    public Guid ExamId { get; set; }
    public DateTime NotifiedAtUtc { get; set; } = DateTime.UtcNow;
}

