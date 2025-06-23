using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExamService.Application.Interfaces;
using Shared.Contracts.ExamService;

namespace ExamService.Application.UseCases.ExamConfigs.Read;

public class GetPublicExamConfigByIdHandler
{
    private readonly IExamConfigRepository _repository;
    private readonly IQuestionsBankRepository _questionsRepository;

    public GetPublicExamConfigByIdHandler(IExamConfigRepository repository, IQuestionsBankRepository questionsRepository)
    {
        _repository = repository;
        _questionsRepository = questionsRepository;
    }

    public async Task<ExamConfigDto?> HandleAsync(Guid id)
    {
        var config = await _repository.GetByIdAsync(id);
        if (config is null) return null;

        var questions = new List<PublicQuestionDto>();
        if (!string.IsNullOrWhiteSpace(string.Join("", config.QuestionIds)))
        {
            // QuestionIds is a List<Guid> but may be loaded as a single string if not materialized by EF
            // Defensive: ensure we split if needed (should be handled by EF, but just in case)
            var questionIds = config.QuestionIds;
            foreach (var qid in questionIds)
            {
                var question = await _questionsRepository.GetByIdAsync(qid);
                if (question != null)
                {
                    questions.Add(new PublicQuestionDto
                    {
                        Id = question.Id,
                        Text = question.Text,
                        Type = question.Type.ToString()
                    });
                }
            }
        }

        return new ExamConfigDto
        {
            Id = config.Id,
            OpenTimeUtc = config.StartDate,
            CloseTimeUtc = config.EndDate,
            Questions = questions
        };
    }
}

