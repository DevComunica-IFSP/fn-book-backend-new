using FnBook.Domain.Entities;
using FnBook.Domain.Interfaces;
using FnBook.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FnBook.Infrastructure.Repositories;

public class NotificationRepository(AppDbContext context) : INotificationRepository
{
    public async Task<IEnumerable<Notification>> GetAllAsync() =>
        await context.Notifications.AsNoTracking().ToListAsync();

    public async Task<Notification?> GetByIdAsync(Guid id) => await context.Notifications.FindAsync(id);

    public async Task<IEnumerable<Notification>> GetByUserIdAsync(Guid userId) =>
        await context.Notifications.AsNoTracking().Where(e => e.UserId == userId).ToListAsync();

    public async Task<Notification> CreateAsync(Notification entity)
    {
        context.Notifications.Add(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    public async Task<Notification> UpdateAsync(Notification entity)
    {
        context.Notifications.Update(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await context.Notifications.FindAsync(id);
        if (entity is null) return false;
        context.Notifications.Remove(entity);
        await context.SaveChangesAsync();
        return true;
    }
}
