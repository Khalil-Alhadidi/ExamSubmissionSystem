using ExamService.Application.Interfaces;
using ExamService.Domain.Entities;

namespace ExamService.Application.UseCases.Subjects.Read;

public class GetSubjectByIdHandler
{
    private readonly ISubjectRepository _repository;

    public GetSubjectByIdHandler(ISubjectRepository repository)
    {
        _repository = repository;
    }

    public async Task<Subject?> HandleAsync(Guid id)
    {
        return await _repository.GetByIdAsync(id);
    }
}
