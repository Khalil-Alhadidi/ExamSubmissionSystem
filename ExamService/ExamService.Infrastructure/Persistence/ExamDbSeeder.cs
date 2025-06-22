using ExamService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExamService.Infrastructure.Persistence;

public static class ExamDbSeeder
{
    public static async Task SeedAsync(ExamDbContext dbContext)
    {
        if (await dbContext.Subjects.AnyAsync()) return;

        var subject = new Subject
        {
            Id = Guid.NewGuid(),
            Name = "Mathematics"
        };

        var questions = new List<QuestionsBank>
        {
            new()
            {
                Id = Guid.NewGuid(),
                SubjectId = subject.Id,
                Text = "2 + 2 = ?",
                Type = QuestionType.TrueFalse,
                CorrectAnswer = "4"
            },
            new()
            {
                Id = Guid.NewGuid(),
                SubjectId = subject.Id,
                Text = "What is the integral of x?",
                Type = QuestionType.Narrative,
                CorrectAnswer = "0.5x^2 + C"
            }
        };

        var exam = new ExamConfig
        {
            Id = Guid.NewGuid(),
            Name = "Math Final Exam",
            SubjectId = subject.Id,
            StartDate = DateTimeOffset.UtcNow.AddDays(-1),
            EndDate = DateTimeOffset.UtcNow.AddDays(3),
            TotalQuestions = 2,
            QuestionIds = questions.Select(q => q.Id).ToList()
        };

        await dbContext.Subjects.AddAsync(subject);
        await dbContext.QuestionsBank.AddRangeAsync(questions);
        await dbContext.ExamConfigs.AddAsync(exam);

        await dbContext.SaveChangesAsync();
    }
}
