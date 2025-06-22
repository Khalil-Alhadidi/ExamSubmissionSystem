using ExamService.Domain.Entities;

namespace ExamService.Application.DTOs.Questions;

public record UpdateQuestionDto(string Text, QuestionType Type, string CorrectAnswer);
