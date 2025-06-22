using ExamService.Application.DTOs.Questions;
using ExamService.Application.DTOs.SubjectsDtos;
using ExamService.Application.Interfaces;
using ExamService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamService.Application.UseCases.Questions.Create;


public class CreateQuestionHandler
{
    private readonly IQuestionsBankRepository _repository;

    public CreateQuestionHandler(IQuestionsBankRepository repository)
    {
        _repository = repository;
    }

    public async Task HandleAsync(CreateQuestionDto dto)
    {
        var question = new QuestionsBank
        {
            Id = Guid.NewGuid(),
            SubjectId = dto.SubjectId,
            Text = dto.Text,
            Type = dto.Type,
            CorrectAnswer = dto.CorrectAnswer
        };

        await _repository.CreateAsync(question);
    }
}
