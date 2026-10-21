using FnBook.Application.DTOs;
using FnBook.Domain.Entities;

namespace FnBook.Application.Mappings;

public static class CommentReportMapping
{
    public static CommentReportResponseDto ToResponseDto(this CommentReport entity) => new()
    {
        Id = entity.Id,
        CommentId = entity.CommentId,
        UserId = entity.UserId,
        Reason = entity.Reason,
        Date = entity.Date,
    };

    public static CommentReport ToEntity(this CreateCommentReportDto dto) => new()
    {
        Id = Guid.NewGuid(),
        CommentId = dto.CommentId,
        UserId = dto.UserId,
        Reason = dto.Reason,
        Date = DateTime.UtcNow,
    };

    public static void ApplyUpdate(this CommentReport entity, UpdateCommentReportDto dto)
    {
        entity.CommentId = dto.CommentId;
        entity.UserId = dto.UserId;
        entity.Reason = dto.Reason;
    }
}
