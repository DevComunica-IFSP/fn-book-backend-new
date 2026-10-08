using FnBook.Application.DTOs;
using FnBook.Domain.Entities;

namespace FnBook.Application.Mappings;

public static class CommentLikeMapping
{
    public static CommentLikeResponseDto ToResponseDto(this CommentLike like) => new()
    {
        UserId = like.UserId,
        CommentId = like.CommentId
    };

    public static CommentLike ToEntity(this CreateCommentLikeDto dto) => new()
    {
        UserId = dto.UserId,
        CommentId = dto.CommentId
    };
}
