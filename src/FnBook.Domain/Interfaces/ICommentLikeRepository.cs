using FnBook.Domain.Entities;

namespace FnBook.Domain.Interfaces;

public interface ICommentLikeRepository
{
    Task<IEnumerable<CommentLike>> GetByCommentIdAsync(Guid commentId);
    Task<bool> ExistsAsync(Guid userId, Guid commentId);
    Task<CommentLike> CreateAsync(CommentLike like);
    Task<bool> DeleteAsync(Guid userId, Guid commentId);
}
