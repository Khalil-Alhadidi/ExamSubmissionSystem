using ExamService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExamService.Infrastructure.Persistence;

public class ExamDbContext : DbContext
{
    public ExamDbContext(DbContextOptions<ExamDbContext> options)
        : base(options)
    {
    }

    public DbSet<Subject> Subjects => Set<Subject>();
    public DbSet<QuestionsBank> QuestionsBank => Set<QuestionsBank>();
    public DbSet<ExamConfig> ExamConfigs => Set<ExamConfig>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Set default schema
        modelBuilder.HasDefaultSchema("Exam");

        modelBuilder.Entity<ExamConfig>()
            .Property(e => e.QuestionIds)
            .HasConversion(
                v => string.Join(',', v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(Guid.Parse).ToList()
            );
    }
}
