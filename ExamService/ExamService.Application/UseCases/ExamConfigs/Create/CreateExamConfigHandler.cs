using ExamService.Application.DTOs;
using ExamService.Application.DTOs.ExamConfigDtos;
using ExamService.Application.Interfaces;
using ExamService.Domain.Entities;

namespace ExamService.Application.UseCases.ExamConfigs.Create;

public class CreateExamConfigHandler
{
    private readonly IExamConfigRepository _repository;

    public CreateExamConfigHandler(IExamConfigRepository repository)
    {
        _repository = repository;
    }

    public async Task HandleAsync(CreateExamConfigDto dto)
    {
        var config = new ExamConfig
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            SubjectId = dto.SubjectId,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            TotalQuestions = dto.TotalQuestions,
            QuestionIds = dto.QuestionIds
        };

        await _repository.CreateAsync(config);
    }
}
