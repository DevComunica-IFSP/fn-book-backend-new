using FnBook.Domain.Entities;

namespace FnBook.Domain.Interfaces;

public interface INotificationRepository
{
    Task<IEnumerable<Notification>> GetAllAsync();
    Task<Notification?> GetByIdAsync(Guid id);
    Task<IEnumerable<Notification>> GetByUserIdAsync(Guid userId);
    Task<Notification> CreateAsync(Notification entity);
    Task<Notification> UpdateAsync(Notification entity);
    Task<bool> DeleteAsync(Guid id);
}
