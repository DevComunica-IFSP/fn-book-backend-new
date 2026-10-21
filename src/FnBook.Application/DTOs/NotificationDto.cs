using System.ComponentModel.DataAnnotations;

namespace FnBook.Application.DTOs;

public record CreateNotificationDto
{
    public Guid UserId { get; init; }
    public Guid CommentId { get; init; }
    [Required]
    [StringLength(50)]
    public string Type { get; init; } = string.Empty;
    [Range(1, int.MaxValue)]
    public int Quantity { get; init; } = 1;
    public bool IsRead { get; init; }
}

public record UpdateNotificationDto : CreateNotificationDto;

public record NotificationResponseDto
{
    public Guid Id { get; init; }
    public Guid UserId { get; init; }
    public Guid CommentId { get; init; }
    public string Type { get; init; } = string.Empty;
    public int Quantity { get; init; } = 1;
    public bool IsRead { get; init; }
    public DateTime UpdatedAt { get; init; }
}
