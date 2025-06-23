using ExamService.Application.DTOs.Questions;
using ExamService.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamService.Application.UseCases.Questions.Delete;

public class DeleteQuestionHandler
{
    private readonly IQuestionsBankRepository _repository;

    public DeleteQuestionHandler(IQuestionsBankRepository repository)
    {
        _repository = repository;
    }

    public async Task HandleAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }
}
