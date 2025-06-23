using ExamService.Application.DTOs.SubjectsDtos;
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

    public async Task<List<ReturnReadDto>> HandleAsync()
    {
        var subjects = await _repository.GetAllAsync();
        return subjects.Select(s => new ReturnReadDto(s.Id, s.Name)).ToList();
    }
}

