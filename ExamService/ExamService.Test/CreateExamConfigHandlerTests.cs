using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ExamService.Application.DTOs.ExamConfigDtos;
using ExamService.Application.Interfaces;
using ExamService.Application.UseCases.ExamConfigs.Create;
using ExamService.Domain.Entities;
using FluentAssertions;
using Moq;
using Xunit;

namespace ExamService.Test
{
    public class CreateExamConfigHandlerTests
    {
        [Fact]
        public async Task HandleAsync_Should_Create_ExamConfig_When_Valid()
        {
            // Arrange
            var repoMock = new Mock<IExamConfigRepository>();
            var questionsRepoMock = new Mock<IQuestionsBankRepository>();
            var subjectId = Guid.NewGuid();
            var questionId = Guid.NewGuid();
            var dto = new CreateExamConfigDto(
                "Test Exam",
                subjectId,
                DateTimeOffset.UtcNow,
                DateTimeOffset.UtcNow.AddHours(1),
                1,
                new List<Guid> { questionId }
            );
            questionsRepoMock.Setup(x => x.GetByIdAsync(questionId)).ReturnsAsync(new QuestionsBank
            {
                Id = questionId,
                SubjectId = subjectId,
                Text = "Q1",
                Type = 0
            });
            var handler = new CreateExamConfigHandler(repoMock.Object, questionsRepoMock.Object);

            // Act
            await handler.HandleAsync(dto);

            // Assert
            repoMock.Verify(x => x.CreateAsync(It.Is<ExamConfig>(c =>
                c.Name == dto.Name &&
                c.SubjectId == dto.SubjectId &&
                c.TotalQuestions == dto.TotalQuestions &&
                c.QuestionIds.Count == 1 &&
                c.QuestionIds[0] == questionId
            )), Times.Once);
        }

        [Fact]
        public async Task HandleAsync_Should_Throw_When_Duplicate_QuestionIds()
        {
            // Arrange
            var repoMock = new Mock<IExamConfigRepository>();
            var questionsRepoMock = new Mock<IQuestionsBankRepository>();
            var subjectId = Guid.NewGuid();
            var questionId = Guid.NewGuid();
            var dto = new CreateExamConfigDto(
                "Test Exam",
                subjectId,
                DateTimeOffset.UtcNow,
                DateTimeOffset.UtcNow.AddHours(1),
                2,
                new List<Guid> { questionId, questionId }
            );
            var handler = new CreateExamConfigHandler(repoMock.Object, questionsRepoMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => handler.HandleAsync(dto));
        }

        [Fact]
        public async Task HandleAsync_Should_Throw_When_Question_Not_Found()
        {
            // Arrange
            var repoMock = new Mock<IExamConfigRepository>();
            var questionsRepoMock = new Mock<IQuestionsBankRepository>();
            var subjectId = Guid.NewGuid();
            var questionId = Guid.NewGuid();
            var dto = new CreateExamConfigDto(
                "Test Exam",
                subjectId,
                DateTimeOffset.UtcNow,
                DateTimeOffset.UtcNow.AddHours(1),
                1,
                new List<Guid> { questionId }
            );
            questionsRepoMock.Setup(x => x.GetByIdAsync(questionId)).ReturnsAsync((QuestionsBank?)null);
            var handler = new CreateExamConfigHandler(repoMock.Object, questionsRepoMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => handler.HandleAsync(dto));
        }

        [Fact]
        public async Task HandleAsync_Should_Throw_When_Question_SubjectId_Mismatch()
        {
            // Arrange
            var repoMock = new Mock<IExamConfigRepository>();
            var questionsRepoMock = new Mock<IQuestionsBankRepository>();
            var subjectId = Guid.NewGuid();
            var questionId = Guid.NewGuid();
            var dto = new CreateExamConfigDto(
                "Test Exam",
                subjectId,
                DateTimeOffset.UtcNow,
                DateTimeOffset.UtcNow.AddHours(1),
                1,
                new List<Guid> { questionId }
            );
            questionsRepoMock.Setup(x => x.GetByIdAsync(questionId)).ReturnsAsync(new QuestionsBank
            {
                Id = questionId,
                SubjectId = Guid.NewGuid(), // mismatch
                Text = "Q1",
                Type = 0
            });
            var handler = new CreateExamConfigHandler(repoMock.Object, questionsRepoMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => handler.HandleAsync(dto));
        }

        [Fact]
        public async Task HandleAsync_Should_Throw_When_TotalQuestions_Mismatch()
        {
            // Arrange
            var repoMock = new Mock<IExamConfigRepository>();
            var questionsRepoMock = new Mock<IQuestionsBankRepository>();
            var subjectId = Guid.NewGuid();
            var questionId1 = Guid.NewGuid();
            var questionId2 = Guid.NewGuid();
            var dto = new CreateExamConfigDto(
                "Test Exam",
                subjectId,
                DateTimeOffset.UtcNow,
                DateTimeOffset.UtcNow.AddHours(1),
                3,
                new List<Guid> { questionId1, questionId2 }
            );
            questionsRepoMock.Setup(x => x.GetByIdAsync(questionId1)).ReturnsAsync(new QuestionsBank
            {
                Id = questionId1,
                SubjectId = subjectId,
                Text = "Q1",
                Type = 0
            });
            questionsRepoMock.Setup(x => x.GetByIdAsync(questionId2)).ReturnsAsync(new QuestionsBank
            {
                Id = questionId2,
                SubjectId = subjectId,
                Text = "Q2",
                Type = 0
            });
            var handler = new CreateExamConfigHandler(repoMock.Object, questionsRepoMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => handler.HandleAsync(dto));
        }
    }
}
