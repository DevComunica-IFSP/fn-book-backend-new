using System.ComponentModel.DataAnnotations;
using FnBook.Application.DTOs;
using FnBook.Application.Interfaces;
using FnBook.Application.Mappings;
using FnBook.Domain.Interfaces;

namespace FnBook.Application.Services;

public class NotificationService(INotificationRepository repository, IUserRepository userRepository, ICommentRepository commentRepository) : INotificationService
{
    public async Task<IEnumerable<NotificationResponseDto>> GetAllAsync() =>
        (await repository.GetAllAsync()).Select(e => e.ToResponseDto());

    public async Task<NotificationResponseDto?> GetByIdAsync(Guid id) =>
        (await repository.GetByIdAsync(id))?.ToResponseDto();

    public async Task<IEnumerable<NotificationResponseDto>> GetByUserIdAsync(Guid userId) =>
        (await repository.GetByUserIdAsync(userId)).Select(e => e.ToResponseDto());

    public async Task<NotificationResponseDto> CreateAsync(CreateNotificationDto dto)
    {
        await ValidateAsync(dto);
        return (await repository.CreateAsync(dto.ToEntity())).ToResponseDto();
    }

    public async Task<NotificationResponseDto?> UpdateAsync(Guid id, UpdateNotificationDto dto)
    {
        var entity = await repository.GetByIdAsync(id);
        if (entity is null) return null;
        await ValidateAsync(dto);
        entity.ApplyUpdate(dto);
        return (await repository.UpdateAsync(entity)).ToResponseDto();
    }

    public Task<bool> DeleteAsync(Guid id) => repository.DeleteAsync(id);

    public async Task<NotificationResponseDto?> MarkAsReadAsync(Guid id)
    {
        var entity = await repository.GetByIdAsync(id);
        if (entity is null) return null;
        entity.IsRead = true;
        entity.UpdatedAt = DateTime.UtcNow;
        return (await repository.UpdateAsync(entity)).ToResponseDto();
    }

    private async Task ValidateAsync(CreateNotificationDto dto)
    {
        Validator.ValidateObject(dto, new ValidationContext(dto), validateAllProperties: true);
        if (dto.UserId == Guid.Empty || await userRepository.GetByIdAsync(dto.UserId) is null)
            throw new ArgumentException("Usuário não encontrado.");
        if (dto.CommentId == Guid.Empty || await commentRepository.GetByIdAsync(dto.CommentId) is null)
            throw new ArgumentException("Comentário não encontrado.");
    }
}
