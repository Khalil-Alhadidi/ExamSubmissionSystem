using MassTransit;
using Shared.Contracts.Events;
using SubmissionService.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubmissionService.Infrastructure.Services;

public class EventPublisher : IEventPublisher
{
    private readonly IPublishEndpoint _publishEndpoint;

    public EventPublisher(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task PublishAsync(AnswersSubmittedEvent @event)
    {
        await _publishEndpoint.Publish(@event);
    }
}
