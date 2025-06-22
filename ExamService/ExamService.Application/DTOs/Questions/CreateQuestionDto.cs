using ExamService.Domain.Entities;
using System.Text.Json.Serialization;

namespace ExamService.Application.DTOs.Questions;

public record CreateQuestionDto(Guid SubjectId, string Text, [property: JsonPropertyName("QuestionType")] QuestionType Type, string CorrectAnswer);
