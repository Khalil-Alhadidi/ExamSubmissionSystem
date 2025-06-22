using ExamService.Application.Interfaces;

namespace ExamService.Application.UseCases.ExamConfigs.Delete;

public class DeleteExamConfigHandler
{
    private readonly IExamConfigRepository _repository;

    public DeleteExamConfigHandler(IExamConfigRepository repository)
    {
        _repository = repository;
    }

    public async Task HandleAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }
}
