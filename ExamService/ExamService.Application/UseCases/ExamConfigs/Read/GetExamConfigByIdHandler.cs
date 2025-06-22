using ExamService.Application.Interfaces;
using ExamService.Domain.Entities;

namespace ExamService.Application.UseCases.ExamConfigs.Read;

public class GetExamConfigByIdHandler
{
    private readonly IExamConfigRepository _repository;

    public GetExamConfigByIdHandler(IExamConfigRepository repository)
    {
        _repository = repository;
    }

    public async Task<ExamConfig?> HandleAsync(Guid id)
    {
        return await _repository.GetByIdAsync(id);
    }
}
