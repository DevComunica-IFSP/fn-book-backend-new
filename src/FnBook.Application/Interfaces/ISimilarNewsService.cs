using FnBook.Application.DTOs;

namespace FnBook.Application.Interfaces;

public interface ISimilarNewsService
{
    Task<IEnumerable<SimilarNewsResponseDto>> GetByNewsIdAsync(Guid newsId);
    Task<SimilarNewsResponseDto?> GetByIdAsync(Guid newsId, Guid similarNewsId);
    Task<SimilarNewsResponseDto?> CreateAsync(CreateSimilarNewsDto dto);
    Task<SimilarNewsResponseDto?> UpdateAsync(Guid newsId, Guid similarNewsId, UpdateSimilarNewsDto dto);
    Task<bool> DeleteAsync(Guid newsId, Guid similarNewsId);
}
