using ExamService.Application.Interfaces;
using ExamService.Domain.Entities;
using ExamService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ExamService.Infrastructure.Repositories;

public class SubjectRepository : ISubjectRepository
{
    private readonly ExamDbContext _context;

    public SubjectRepository(ExamDbContext context)
    {
        _context = context;
    }

    public async Task<List<Subject>> GetAllAsync() =>
        await _context.Subjects.ToListAsync();

    public async Task<Subject?> GetByIdAsync(Guid id) =>
        await _context.Subjects.FindAsync(id);

    public async Task CreateAsync(Subject subject)
    {
        _context.Subjects.Add(subject);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Subject subject)
    {
        _context.Subjects.Update(subject);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var subject = await _context.Subjects.FindAsync(id);
        if (subject != null)
        {
            _context.Subjects.Remove(subject);
            await _context.SaveChangesAsync();
        }
    }
}
