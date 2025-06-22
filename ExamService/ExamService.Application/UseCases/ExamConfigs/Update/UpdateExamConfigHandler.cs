using ExamService.Application.DTOs;
using ExamService.Application.DTOs.ExamConfigDtos;
using ExamService.Application.Interfaces;

namespace ExamService.Application.UseCases.ExamConfigs.Update;

public class UpdateExamConfigHandler
{
    private readonly IExamConfigRepository _repository;

    public UpdateExamConfigHandler(IExamConfigRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> HandleAsync(Guid id, UpdateExamConfigDto dto)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing is null) return false;

        existing.Name = dto.Name;
        existing.SubjectId = dto.SubjectId;
        existing.StartDate = dto.StartDate;
        existing.EndDate = dto.EndDate;
        existing.TotalQuestions = dto.TotalQuestions;
        existing.QuestionIds = dto.QuestionIds;

        await _repository.UpdateAsync(existing);
        return true;
    }
}
