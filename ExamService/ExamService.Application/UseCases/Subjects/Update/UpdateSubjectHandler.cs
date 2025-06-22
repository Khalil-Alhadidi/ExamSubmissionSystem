using ExamService.Application.DTOs.SubjectsDtos;
using ExamService.Application.Interfaces;

namespace ExamService.Application.UseCases.Subjects.Update;



public class UpdateSubjectHandler
{
    private readonly ISubjectRepository _repository;

    public UpdateSubjectHandler(ISubjectRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> HandleAsync(Guid id, UpdateSubjectDto dto)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing is null) return false;

        existing.Name = dto.Name;
        await _repository.UpdateAsync(existing);
        return true;
    }
}
