﻿using ExamService.Application.Interfaces;
using ExamService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Shared.Entities;
using Shared.Interfaces;

namespace ExamService.Infrastructure.Persistence;

public class ExamDbContext : DbContext
{
    private readonly ICurrentUserService _currentUser;

    public ExamDbContext(DbContextOptions<ExamDbContext> options, ICurrentUserService currentUser)
        : base(options)
    {
        _currentUser = currentUser;
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
        
        // for soft delete functionality filter
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(AuditableEntity).IsAssignableFrom(entityType.ClrType))
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
