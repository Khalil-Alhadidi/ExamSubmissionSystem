using ExamService.Application.DTOs.ExamConfigDtos;
using ExamService.Application.Interfaces;

public class GetExamWithQuestionsHandler
{
    private readonly IExamConfigRepository _repository;

    public GetExamWithQuestionsHandler(IExamConfigRepository repository)
    {
        _repository = repository;
    }

    public async Task<ExamWithQuestionsDto?> HandleAsync(Guid examId)
    {
        return await _repository.GetWithQuestionsAsync(examId);
    }
}
