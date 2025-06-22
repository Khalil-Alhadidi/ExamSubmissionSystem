using ExamService.Application.Interfaces;
using ExamService.Domain.Entities;

namespace ExamService.Application.UseCases.Subjects.Read;


public class GetSubjectsHandler
{
    private readonly ISubjectRepository _repository;

    public GetSubjectsHandler(ISubjectRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<Subject>> HandleAsync()
    {
        return await _repository.GetAllAsync();
    }
}

