using SubmissionService.Application.Interfaces;
using SubmissionService.Domain.Entities;
using SubmissionService.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SubmissionService.Infrastructure.Repositories;

public class SubmissionRepository : ISubmissionRepository
{
    private readonly SubmissionDbContext _context;

    public SubmissionRepository(SubmissionDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Submission submission)
    {
        _context.Submissions.Add(submission);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(Guid studentId, Guid examId)
    {
        return await _context.Submissions.AnyAsync(s => s.StudentId == studentId && s.ExamId == examId);
    }
}
