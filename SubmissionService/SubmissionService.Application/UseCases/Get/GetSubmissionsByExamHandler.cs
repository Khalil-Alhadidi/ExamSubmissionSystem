using SubmissionService.Application.Interfaces;
using SubmissionService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubmissionService.Application.UseCases.Get;

public class GetSubmissionsByExamHandler
{
    private readonly ISubmissionRepository _repository;

    public GetSubmissionsByExamHandler(ISubmissionRepository repository)
    {
        _repository = repository;
    }

    public Task<List<Submission>> HandleAsync(Guid examId)
    {
        return _repository.GetByExamIdAsync(examId);
    }
}
