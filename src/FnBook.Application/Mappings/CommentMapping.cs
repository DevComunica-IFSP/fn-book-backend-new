using FnBook.Application.DTOs;
using FnBook.Domain.Entities;

namespace FnBook.Application.Mappings;

public static class CommentMapping
{
    public static CommentResponseDto ToResponseDto(this Comment comment) => new()
    {
        Id = comment.Id,
        UserId = comment.UserId,
        NewsId = comment.NewsId,
        ParentCommentId = comment.ParentCommentId,
        Text = comment.Text,
        Date = comment.Date
    };

    public static Comment ToEntity(this CreateCommentDto dto) => new()
    {
        Id = Guid.NewGuid(),
        UserId = dto.UserId,
        NewsId = dto.NewsId,
        ParentCommentId = dto.ParentCommentId,
        Text = dto.Text,
        Date = DateTime.UtcNow
    };

    public static void ApplyUpdate(this Comment comment, UpdateCommentDto dto)
    {
        comment.UserId = dto.UserId;
        comment.NewsId = dto.NewsId;
        comment.ParentCommentId = dto.ParentCommentId;
        comment.Text = dto.Text;
    }
}
