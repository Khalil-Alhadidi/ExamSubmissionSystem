using ExamService.Application.Interfaces;

namespace ExamService.Application.UseCases.Subjects.Delete;

public class DeleteSubjectHandler
{
    private readonly ISubjectRepository _repository;

    public DeleteSubjectHandler(ISubjectRepository repository)
    {
        _repository = repository;
    }

    public async Task HandleAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }
}
