using ExamService.Application.Interfaces;
using ExamService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamService.Application.UseCases.Questions.Read;

public class GetQuestionByIdHandler
{

    private readonly IQuestionsBankRepository _repository;

    public GetQuestionByIdHandler(IQuestionsBankRepository repository)
    {
        _repository = repository;
    }

    public async Task<QuestionsBank?> HandleAsync(Guid id)
    {
        return await _repository.GetByIdAsync(id);
    }
}
