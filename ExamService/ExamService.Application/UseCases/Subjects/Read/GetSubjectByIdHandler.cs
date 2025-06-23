using ExamService.Application.DTOs.SubjectsDtos;
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

    public async Task<ReturnReadDto?> HandleAsync(Guid id)
    {
        var subject = await _repository.GetByIdAsync(id);
        return subject is null ? null : new ReturnReadDto(subject.Id, subject.Name);
    }
}
