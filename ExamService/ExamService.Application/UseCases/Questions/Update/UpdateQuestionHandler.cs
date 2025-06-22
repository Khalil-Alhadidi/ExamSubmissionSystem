using ExamService.Application.DTOs.Questions;
using ExamService.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamService.Application.UseCases.Questions.Update;

public class UpdateQuestionHandler
{
    private readonly IQuestionsBankRepository _repository;

    public UpdateQuestionHandler(IQuestionsBankRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> HandleAsync(Guid id, UpdateQuestionDto dto)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing is null) return false;

        existing.Text = dto.Text;
        existing.Type = dto.Type;
        existing.CorrectAnswer = dto.CorrectAnswer;

        await _repository.UpdateAsync(existing);
        return true;
    }
}
