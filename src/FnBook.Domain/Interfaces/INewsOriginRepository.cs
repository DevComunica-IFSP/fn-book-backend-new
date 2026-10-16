using FnBook.Domain.Entities;

namespace FnBook.Domain.Interfaces;

public interface INewsOriginRepository
{
    Task<IEnumerable<NewsOrigin>> GetAllAsync();
    Task<NewsOrigin?> GetByIdAsync(Guid id);
    Task<IEnumerable<NewsOrigin>> GetByNewsIdAsync(Guid newsId);
    Task<NewsOrigin> CreateAsync(NewsOrigin origin);
    Task<NewsOrigin> UpdateAsync(NewsOrigin origin);
    Task<bool> DeleteAsync(Guid id);
}
