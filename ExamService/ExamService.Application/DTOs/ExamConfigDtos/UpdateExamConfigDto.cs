namespace ExamService.Application.DTOs.ExamConfigDtos;

public record UpdateExamConfigDto(
    string Name,
    Guid SubjectId,
    DateTimeOffset StartDate,
    DateTimeOffset EndDate,
    int TotalQuestions,
    List<Guid> QuestionIds
);