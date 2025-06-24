using Microsoft.Extensions.Logging;
using NotificationService.Application.Interfaces;
using NotificationService.Domain.Entities;
using Shared.Contracts.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Application.UseCases;

public class AnswerSubmittedHandler : IAnswerSubmittedHandler
{
    private readonly INotificationLogRepository _repo;
    private readonly ILogger<AnswerSubmittedHandler> _logger;

    public AnswerSubmittedHandler(INotificationLogRepository repo, ILogger<AnswerSubmittedHandler> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    public async Task HandleAsync(AnswersSubmittedEvent message)
    {
        var log = new NotificationLog
        {
            SubmissionId = message.SubmissionId,
            StudentId = message.StudentId,
            ExamId = message.ExamId
        };

        await _repo.AddAsync(log);
        _logger.LogInformation($"Notification logged for submission {message.SubmissionId}");
    }
}
