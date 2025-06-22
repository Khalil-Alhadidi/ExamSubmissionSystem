namespace ExamService.Application.DTOs.ExamConfigDtos;

public record CreateExamConfigDto(
    string Name,
    Guid SubjectId,
    DateTimeOffset StartDate,
    DateTimeOffset EndDate,
    int TotalQuestions,
    List<Guid> QuestionIds
);