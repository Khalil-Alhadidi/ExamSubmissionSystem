using ExamService.Application.DTOs;
using ExamService.Application.DTOs.ExamConfigDtos;
using ExamService.Application.Interfaces;
using ExamService.Domain.Entities;

namespace ExamService.Application.UseCases.ExamConfigs.Create;

public class CreateExamConfigHandler
{
    private readonly IExamConfigRepository _repository;
    private readonly IQuestionsBankRepository _questionsBankRepository;

    public CreateExamConfigHandler(IExamConfigRepository repository, IQuestionsBankRepository questionsBankRepository)
    {
        _repository = repository;
        _questionsBankRepository = questionsBankRepository;
    }

    public async Task HandleAsync(CreateExamConfigDto dto)
    {
        // Ensure no duplicate question IDs
        if (dto.QuestionIds.Count != dto.QuestionIds.Distinct().Count())
            throw new InvalidOperationException("Duplicate question IDs are not allowed.");

        // Fetch all questions by IDs
        var questions = new List<QuestionsBank>();
        foreach (var qid in dto.QuestionIds)
        {
            var question = await _questionsBankRepository.GetByIdAsync(qid);
            if (question == null)
                throw new InvalidOperationException($"Question with ID {qid} does not exist.");
            if (question.SubjectId != dto.SubjectId)
                throw new InvalidOperationException($"Question {qid} does not belong to the specified subject.");
            questions.Add(question);
        }

        // Ensure total questions matches
        if (questions.Count != dto.TotalQuestions)
            throw new InvalidOperationException("TotalQuestions does not match the number of valid questions provided.");

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
