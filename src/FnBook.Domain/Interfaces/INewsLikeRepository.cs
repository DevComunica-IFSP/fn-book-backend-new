using FnBook.Domain.Entities;

namespace FnBook.Domain.Interfaces;

public interface INewsLikeRepository
{
    Task<IEnumerable<NewsLike>> GetByNewsIdAsync(Guid newsId);
    Task<bool> ExistsAsync(Guid userId, Guid newsId);
    Task<NewsLike> CreateAsync(NewsLike like);
    Task<bool> DeleteAsync(Guid userId, Guid newsId);
}
