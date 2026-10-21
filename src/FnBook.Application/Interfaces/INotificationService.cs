using FnBook.Application.DTOs;

namespace FnBook.Application.Interfaces;

public interface INotificationService
{
    Task<IEnumerable<NotificationResponseDto>> GetAllAsync();
    Task<NotificationResponseDto?> GetByIdAsync(Guid id);
    Task<IEnumerable<NotificationResponseDto>> GetByUserIdAsync(Guid userId);
    Task<NotificationResponseDto> CreateAsync(CreateNotificationDto dto);
    Task<NotificationResponseDto?> UpdateAsync(Guid id, UpdateNotificationDto dto);
    Task<bool> DeleteAsync(Guid id);
    Task<NotificationResponseDto?> MarkAsReadAsync(Guid id);
}
