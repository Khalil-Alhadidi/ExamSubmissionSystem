using ExamService.Domain.Entities;

namespace ExamService.Application.DTOs.Questions;

public record CreateQuestionDto(Guid SubjectId, string Text, QuestionType Type, string CorrectAnswer);
