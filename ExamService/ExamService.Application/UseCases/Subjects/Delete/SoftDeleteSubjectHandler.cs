using ExamService.Application.Interfaces;

namespace ExamService.Application.UseCases.Subjects.Delete;

public class SoftDeleteSubjectHandler
{
    private readonly ISubjectRepository _repository;

    public SoftDeleteSubjectHandler(ISubjectRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> HandleAsync(Guid id)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing is null) return false;

        existing.IsDeleted = true;
        existing.DeletedAtUtc = DateTime.UtcNow;
        await _repository.UpdateAsync(existing);
        return true;
    }
}