using SubmissionService.Application.Interfaces;
using SubmissionService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubmissionService.Application.UseCases.Get;

public class GetAllSubmissionsHandler
{
    private readonly ISubmissionRepository _repository;

    public GetAllSubmissionsHandler(ISubmissionRepository repository)
    {
        _repository = repository;
    }

    public Task<List<Submission>> HandleAsync()
    {
        return _repository.GetAllAsync();
    }
}
