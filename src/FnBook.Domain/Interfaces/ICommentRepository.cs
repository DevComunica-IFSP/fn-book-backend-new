using FnBook.Domain.Entities;

namespace FnBook.Domain.Interfaces;

public interface ICommentRepository
{
    Task<IEnumerable<Comment>> GetAllAsync();
    Task<Comment?> GetByIdAsync(Guid id);
    Task<Comment> CreateAsync(Comment comment);
    Task<Comment> UpdateAsync(Comment comment);
    Task<bool> DeleteAsync(Guid id);
}
