using FnBook.Domain.Entities;

namespace FnBook.Domain.Interfaces;

public interface ICommentReportRepository
{
    Task<IEnumerable<CommentReport>> GetAllAsync();
    Task<CommentReport?> GetByIdAsync(Guid id);
    Task<IEnumerable<CommentReport>> GetByCommentIdAsync(Guid commentId);
    Task<CommentReport> CreateAsync(CommentReport entity);
    Task<CommentReport> UpdateAsync(CommentReport entity);
    Task<bool> DeleteAsync(Guid id);
}
