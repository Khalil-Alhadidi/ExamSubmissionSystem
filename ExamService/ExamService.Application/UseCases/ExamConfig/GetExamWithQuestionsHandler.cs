using ExamService.Application.DTOs.ExamConfigDtos;
using ExamService.Application.Interfaces;

namespace ExamService.Application.UseCases.ExamConfig;

public class GetExamWithQuestionsHandler
{
    private readonly IExamReadService _service;

    public GetExamWithQuestionsHandler(IExamReadService service)
    {
        _service = service;
    }

    public async Task<ExamWithQuestionsDto?> HandleAsync(Guid examId)
    {
        var exam = await _service.GetExamWithQuestionsAsync(examId);
        if (exam == null) return null;

        var questions = await _service.GetQuestionsAsync(exam.SubjectId);
        var matched = questions.Where(q => exam.QuestionIds.Contains(q.Id)).ToList();

        return new ExamWithQuestionsDto
        {
            Exam = exam,
            Questions = matched
        };
    }
}
