using ExamService.Domain.Entities;
using ExamService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using ExamService.Application.Interfaces;
namespace ExamService.Infrastructure.Repositories;

public class QuestionsBankRepository : IQuestionsBankRepository
{
    private readonly ExamDbContext _context;

    public QuestionsBankRepository(ExamDbContext context)
    {
        _context = context;
    }

    public async Task<List<QuestionsBank>> GetAllAsync() =>
        await _context.QuestionsBank.ToListAsync();

    public async Task<QuestionsBank?> GetByIdAsync(Guid id) =>
        await _context.QuestionsBank.FindAsync(id);

    public async Task<List<QuestionsBank>> GetBySubjectIdAsync(Guid subjectId) =>
        await _context.QuestionsBank.Where(q => q.SubjectId == subjectId).ToListAsync();

    public async Task CreateAsync(QuestionsBank question)
    {
        _context.QuestionsBank.Add(question);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(QuestionsBank question)
    {
        _context.QuestionsBank.Update(question);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var q = await _context.QuestionsBank.FindAsync(id);
        if (q is not null)
        {
            _context.QuestionsBank.Remove(q);
            await _context.SaveChangesAsync();
        }
    }
}

