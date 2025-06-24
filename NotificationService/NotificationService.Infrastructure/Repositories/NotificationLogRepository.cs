using NotificationService.Application.Interfaces;
using NotificationService.Domain.Entities;
using NotificationService.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Infrastructure.Repositories;

public class NotificationLogRepository : INotificationLogRepository
{
    private readonly NotificationDbContext _db;

    public NotificationLogRepository(NotificationDbContext db)
    {
        _db = db;
    }

    public async Task AddAsync(NotificationLog log)
    {
        _db.NotificationLogs.Add(log);
        await _db.SaveChangesAsync();
    }
}