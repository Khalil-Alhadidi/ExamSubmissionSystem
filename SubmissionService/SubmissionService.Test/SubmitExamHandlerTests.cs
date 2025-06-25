using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using SubmissionService.Application.DTOs;
using SubmissionService.Application.Interfaces;
using SubmissionService.Application.UseCases.Create;
using Shared.Contracts.ExamService;
using SubmissionService.Domain.Entities;
using Shared.Contracts.Events;
using Xunit;

namespace SubmissionService.Test
{
    public class SubmitExamHandlerTests
    {
        [Fact]
        public async Task HandleAsync_Should_SaveSubmission_And_PublishEvent_When_Valid()
        {
            // Arrange
            var repoMock = new Mock<ISubmissionRepository>();
            var publisherMock = new Mock<IEventPublisher>();
            repoMock.Setup(r => r.ExistsAsync(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(false);
            var handler = new SubmitExamHandler(repoMock.Object, publisherMock.Object);

            var studentId = Guid.NewGuid();
            var examId = Guid.NewGuid();
            var questionId = Guid.NewGuid();
            var configQuestions = new List<PublicQuestionDto>
            {
                new PublicQuestionDto { Id = questionId, Text = "Q1", Type = "TrueFalse" }
            };
            var request = new SubmitExamRequest
            {
                Answers = new List<AnswerDto>
                {
                    new AnswerDto { QuestionId = questionId, AnswerValue = "True" }
                }
            };

            // Act
            var result = await handler.HandleAsync(request, studentId, examId, configQuestions);

            // Assert
            repoMock.Verify(r => r.AddAsync(It.Is<Submission>(s =>
                s.StudentId == studentId &&
                s.ExamId == examId &&
                s.Answers.Count == 1 &&
                s.Answers[0].QuestionId == questionId &&
                s.Answers[0].AnswerValue == "True"
            )), Times.Once);
            publisherMock.Verify(p => p.PublishAsync(It.IsAny<AnswersSubmittedEvent>()), Times.Once);
            result.Should().NotBeEmpty();
        }

        [Fact]
        public async Task HandleAsync_Should_Throw_When_DuplicateSubmission()
        {
            // Arrange
            var repoMock = new Mock<ISubmissionRepository>();
            var publisherMock = new Mock<IEventPublisher>();
            repoMock.Setup(r => r.ExistsAsync(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(true);
            var handler = new SubmitExamHandler(repoMock.Object, publisherMock.Object);

            var studentId = Guid.NewGuid();
            var examId = Guid.NewGuid();
            var configQuestions = new List<PublicQuestionDto>();
            var request = new SubmitExamRequest();

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                handler.HandleAsync(request, studentId, examId, configQuestions));
        }
    }
}
