using FnBook.Domain.Entities;

namespace FnBook.Domain.Interfaces;

public interface ISimilarNewsRepository
{
    Task<IEnumerable<SimilarNews>> GetByNewsIdAsync(Guid newsId);
    Task<SimilarNews?> GetByIdAsync(Guid newsId, Guid similarNewsId);
    Task<SimilarNews> CreateAsync(SimilarNews entity);
    Task<SimilarNews> UpdateAsync(SimilarNews entity);
    Task<bool> DeleteAsync(Guid newsId, Guid similarNewsId);
}
