using SubmissionService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubmissionService.Application.Interfaces;

public interface ISubmissionRepository
{
    Task AddAsync(Submission submission);
    Task<bool> ExistsAsync(Guid studentId, Guid examId);

    Task<List<Submission>> GetAllAsync();
    Task<List<Submission>> GetByStudentIdAsync(Guid studentId);
    Task<List<Submission>> GetByExamIdAsync(Guid examId);
}
