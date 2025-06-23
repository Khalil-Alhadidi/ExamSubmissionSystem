using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubmissionService.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;
using Shared.Entities;
using Shared.Interfaces;
using SubmissionService.Domain.Entities;
using System.Linq.Expressions;

public class SubmissionDbContext : DbContext
{
    private readonly ICurrentUserService _currentUser;
    public SubmissionDbContext(DbContextOptions<SubmissionDbContext> options, ICurrentUserService currentUser)
        : base(options)
    {
        _currentUser = currentUser;
    }
    

    public DbSet<Submission> Submissions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("Submission");

        modelBuilder.Entity<Submission>(submission =>
        {
            submission.HasKey(s => s.Id);
            submission.Property(s => s.StudentId).IsRequired();
            submission.Property(s => s.ExamId).IsRequired();
            submission.Property(s => s.SubmittedAtUtc).IsRequired();

            submission.OwnsMany(s => s.Answers, answers =>
            {
                answers.WithOwner().HasForeignKey("SubmissionId");
                answers.Property<Guid>("Id"); // required for EF Core tracking
                answers.HasKey("Id");
                answers.Property(a => a.QuestionId).IsRequired();
                answers.Property(a => a.QuestionType).IsRequired();
                answers.Property(a => a.SelectedOption);
                answers.Property(a => a.NarrativeAnswerText);
                answers.ToTable("Answers");
            });

            submission.ToTable("Submissions");
        });

        // for soft delete functionality filter, skip owned types
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            // Only apply filter to root entities (not owned)
            if (typeof(AuditableEntity).IsAssignableFrom(entityType.ClrType) && entityType.FindOwnership() == null)
            {
                modelBuilder.Entity(entityType.ClrType)
                    .HasQueryFilter(GetIsDeletedRestriction(entityType.ClrType));
            }
        }
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // UTC time and user ID for auditing
        var now = DateTime.UtcNow;
        var user = _currentUser.UserId ?? "system";

        foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAtUtc = now;
                    entry.Entity.CreatedBy = user;
                    break;

                case EntityState.Modified:
                    entry.Entity.ModifiedAtUtc = now;
                    entry.Entity.ModifiedBy = user;
                    break;

                case EntityState.Deleted:
                    // Soft delete
                    entry.State = EntityState.Modified;
                    entry.Entity.DeletedAtUtc = now;
                    entry.Entity.IsDeleted = true;
                    entry.Entity.ModifiedBy = user;
                    break;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }


    private static LambdaExpression GetIsDeletedRestriction(Type type)
    {
        var param = Expression.Parameter(type, "e");
        var prop = Expression.Property(param, nameof(AuditableEntity.IsDeleted));
        var condition = Expression.Equal(prop, Expression.Constant(false));
        return Expression.Lambda(condition, param);
    }
}

