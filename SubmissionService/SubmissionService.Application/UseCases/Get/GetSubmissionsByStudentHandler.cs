using SubmissionService.Application.Interfaces;
using SubmissionService.Domain.Entities;

namespace SubmissionService.Application.UseCases.Get;

public class GetSubmissionsByStudentHandler
{
    private readonly ISubmissionRepository _repository;

    public GetSubmissionsByStudentHandler(ISubmissionRepository repository)
    {
        _repository = repository;
    }

    public Task<List<Submission>> HandleAsync(Guid studentId)
    {
        return _repository.GetByStudentIdAsync(studentId);
    }
}

