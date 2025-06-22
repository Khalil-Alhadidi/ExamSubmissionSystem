using ExamService.Application.Interfaces;
using ExamService.Domain.Entities;

namespace ExamService.Application.UseCases.ExamConfigs.Read;

public class GetExamConfigsHandler
{
    private readonly IExamConfigRepository _repository;

    public GetExamConfigsHandler(IExamConfigRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<ExamConfig>> HandleAsync()
    {
        return await _repository.GetAllAsync();
    }
}
