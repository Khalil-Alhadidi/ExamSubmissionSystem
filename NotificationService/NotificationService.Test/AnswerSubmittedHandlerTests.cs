using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NotificationService.Application.Interfaces;
using NotificationService.Application.UseCases;
using NotificationService.Domain.Entities;
using Shared.Contracts.Events;
using Xunit;

namespace NotificationService.Test
{
    
    public class AnswerSubmittedHandlerTests
    {
        [Fact]
        public async Task HandleAsync_ShouldAddNotificationLogAndLogInformation()
        {
            // Arrange
            var repoMock = new Mock<INotificationLogRepository>();
            var loggerMock = new Mock<ILogger<AnswerSubmittedHandler>>();
            var handler = new AnswerSubmittedHandler(repoMock.Object, loggerMock.Object);

            var evt = new AnswersSubmittedEvent
            {
                SubmissionId = Guid.NewGuid(),
                StudentId = Guid.NewGuid(),
                ExamId = Guid.NewGuid(),
                SubmittedAtUtc = DateTimeOffset.UtcNow
            };

            // Act
            await handler.HandleAsync(evt);

            // Assert
            repoMock.Verify(r => r.AddAsync(It.Is<NotificationLog>(l =>
                l.SubmissionId == evt.SubmissionId &&
                l.StudentId == evt.StudentId &&
                l.ExamId == evt.ExamId)), Times.Once);

            loggerMock.Verify(l => l.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains($"Notification logged for submission {evt.SubmissionId}")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Once);
        }
    }
}
