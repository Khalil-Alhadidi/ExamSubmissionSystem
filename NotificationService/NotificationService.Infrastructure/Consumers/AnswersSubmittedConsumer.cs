using Microsoft.Extensions.Logging;
using NotificationService.Application.Interfaces;
using Shared.Contracts.Events;
using MassTransit;

namespace NotificationService.Infrastructure.Consumers;

public class AnswersSubmittedConsumer : IConsumer<AnswersSubmittedEvent>
{
    private readonly ILogger<AnswersSubmittedConsumer> _logger;
    private readonly IAnswerSubmittedHandler _handler;

    public AnswersSubmittedConsumer(
        ILogger<AnswersSubmittedConsumer> logger,
        IAnswerSubmittedHandler handler)
    {
        _logger = logger;
        _handler = handler;
    }

    public async Task Consume(ConsumeContext<AnswersSubmittedEvent> context)
    {
        await _handler.HandleAsync(context.Message);
        _logger.LogInformation("Consumed submission {SubmissionId} set for grading.. ", context.Message.SubmissionId);
    }
}