using FnBook.Domain.Entities;

namespace FnBook.Domain.Interfaces;

public interface INewsRepository
{
    Task<IEnumerable<News>> GetAllAsync();
    Task<News?> GetByIdAsync(Guid id);
    Task<News> CreateAsync(News news);
    Task<News> UpdateAsync(News news);
    Task<bool> DeleteAsync(Guid id);
}
