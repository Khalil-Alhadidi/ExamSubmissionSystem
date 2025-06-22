using ExamService.Application.DTOs.SubjectsDtos;
using ExamService.Application.Interfaces;
using ExamService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamService.Application.UseCases.Subjects.Create;


public class CreateSubjectHandler
{
    private readonly ISubjectRepository _repo;

    public CreateSubjectHandler(ISubjectRepository repo)
    {
        _repo = repo;
    }

    public async Task HandleAsync(CreateSubjectDto dto)
    {
        var subject = new Subject
        {
            Id = Guid.NewGuid(),
            Name = dto.Name
        };

        await _repo.CreateAsync(subject);
    }
}
