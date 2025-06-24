using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Shared.Contracts.Events;


public class AnswersSubmittedEvent
{
    public Guid SubmissionId { get; set; }
    public Guid StudentId { get; set; }
    public Guid ExamId { get; set; }
    public DateTime SubmittedAtUtc { get; set; }
}

