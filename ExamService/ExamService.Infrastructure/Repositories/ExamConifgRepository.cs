using ExamService.Application.DTOs.ExamConfigDtos;
using ExamService.Application.Interfaces;
using ExamService.Domain.Entities;
using ExamService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamService.Infrastructure.Repositories;

public class ExamConfigRepository : IExamConfigRepository
{
    private readonly ExamDbContext _context;

    public ExamConfigRepository(ExamDbContext context)
    {
        _context = context;
    }

    public async Task<List<ExamConfig>> GetAllAsync()
    {
        return await _context.ExamConfigs.ToListAsync();
    }

    public async Task<ExamConfig?> GetByIdAsync(Guid id)
    {
        return await _context.ExamConfigs.FindAsync(id);
    }

    public async Task CreateAsync(ExamConfig config)
    {
        _context.ExamConfigs.Add(config);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(ExamConfig config)
    {
        _context.ExamConfigs.Update(config);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var config = await _context.ExamConfigs.FindAsync(id);
        if (config is not null)
        {
            _context.ExamConfigs.Remove(config);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<ExamWithQuestionsDto?> GetWithQuestionsAsync(Guid examId)
    {
        var exam = await _context.ExamConfigs.FindAsync(examId);
        if (exam is null) return null;

        var questions = await _context.QuestionsBank
            .Where(q => exam.QuestionIds.Contains(q.Id))
            .ToListAsync();

        return new ExamWithQuestionsDto
        {
            Id = exam.Id,
            Name = exam.Name,
            SubjectId = exam.SubjectId,
            StartDate = exam.StartDate,
            EndDate = exam.EndDate,
            TotalQuestions = exam.TotalQuestions,
            Questions = questions
        };
    }

}