using FnBook.Application.DTOs;

namespace FnBook.Application.Interfaces;

public interface ICommentLikeService
{
    Task<IEnumerable<CommentLikeResponseDto>> GetByCommentIdAsync(Guid commentId);
    Task<CommentLikeResponseDto?> CreateAsync(CreateCommentLikeDto dto);
    Task<bool> DeleteAsync(Guid userId, Guid commentId);
}
