using Shared.Contracts.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubmissionService.Application.Interfaces;

public interface IEventPublisher
{
    Task PublishAsync(AnswersSubmittedEvent @event);
}