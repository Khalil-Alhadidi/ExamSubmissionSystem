using ExamService.Application.Interfaces;
using ExamService.Domain.Entities;
using ExamService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ExamService.Infrastructure.Services;

public class ExamReadService : IExamReadService
{
    private readonly ExamDbContext _context;

    public ExamReadService(ExamDbContext context)
    {
        _context = context;
    }

    

    public async Task<List<QuestionsBank>> GetQuestionsAsync(Guid subjectId)
    {
        return await _context.QuestionsBank
            .Where(q => q.SubjectId == subjectId)
            .ToListAsync();
    }

    public async Task<ExamConfig?> GetExamWithQuestionsAsync(Guid examId)
    {
        return await _context.ExamConfigs.FirstOrDefaultAsync(e => e.Id == examId);
    }


}

