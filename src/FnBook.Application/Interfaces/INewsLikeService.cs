using FnBook.Application.DTOs;

namespace FnBook.Application.Interfaces;

public interface INewsLikeService
{
    Task<IEnumerable<NewsLikeResponseDto>> GetByNewsIdAsync(Guid newsId);
    Task<NewsLikeResponseDto?> CreateAsync(CreateNewsLikeDto dto);
    Task<bool> DeleteAsync(Guid userId, Guid newsId);
}
