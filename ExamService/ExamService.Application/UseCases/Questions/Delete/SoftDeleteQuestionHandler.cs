using ExamService.Application.Interfaces;

namespace ExamService.Application.UseCases.Questions.Delete;

public class SoftDeleteQuestionHandler
{
    private readonly IQuestionsBankRepository _repository;

    public SoftDeleteQuestionHandler(IQuestionsBankRepository repository)
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
