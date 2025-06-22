using ExamService.Application.Interfaces;
using ExamService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamService.Application.UseCases.Questions.Read;

public class GetQuestionsHandler
{
    private readonly IQuestionsBankRepository _repository;

    public GetQuestionsHandler(IQuestionsBankRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<QuestionsBank>> HandleAsync()
    {
        return await _repository.GetAllAsync();
    }
}
