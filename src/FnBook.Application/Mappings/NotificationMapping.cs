using FnBook.Application.DTOs;
using FnBook.Domain.Entities;

namespace FnBook.Application.Mappings;

public static class NotificationMapping
{
    public static NotificationResponseDto ToResponseDto(this Notification entity) => new()
    {
        Id = entity.Id,
        UserId = entity.UserId,
        CommentId = entity.CommentId,
        Type = entity.Type,
        Quantity = entity.Quantity,
        IsRead = entity.IsRead,
        UpdatedAt = entity.UpdatedAt,
    };

    public static Notification ToEntity(this CreateNotificationDto dto) => new()
    {
        Id = Guid.NewGuid(),
        UserId = dto.UserId,
        CommentId = dto.CommentId,
        Type = dto.Type,
        Quantity = dto.Quantity,
        IsRead = dto.IsRead,
        UpdatedAt = DateTime.UtcNow,
    };

    public static void ApplyUpdate(this Notification entity, UpdateNotificationDto dto)
    {
        entity.UserId = dto.UserId;
        entity.CommentId = dto.CommentId;
        entity.Type = dto.Type;
        entity.Quantity = dto.Quantity;
        entity.IsRead = dto.IsRead;
        entity.UpdatedAt = DateTime.UtcNow;
    }
}
