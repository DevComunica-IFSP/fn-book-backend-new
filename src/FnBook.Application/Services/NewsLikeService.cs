using FnBook.Application.DTOs;
using FnBook.Application.Interfaces;
using FnBook.Application.Mappings;
using FnBook.Domain.Interfaces;

namespace FnBook.Application.Services;

public class NewsLikeService : INewsLikeService
{
    private readonly INewsLikeRepository _newsLikeRepository;

    public NewsLikeService(INewsLikeRepository newsLikeRepository)
    {
        _newsLikeRepository = newsLikeRepository;
    }

    public async Task<IEnumerable<NewsLikeResponseDto>> GetByNewsIdAsync(Guid newsId)
    {
        var likes = await _newsLikeRepository.GetByNewsIdAsync(newsId);
        return likes.Select(l => l.ToResponseDto());
    }

    public async Task<NewsLikeResponseDto?> CreateAsync(CreateNewsLikeDto dto)
    {
        if (await _newsLikeRepository.ExistsAsync(dto.UserId, dto.NewsId))
            return null;

        var like = dto.ToEntity();
        var created = await _newsLikeRepository.CreateAsync(like);
        return created.ToResponseDto();
    }

    public async Task<bool> DeleteAsync(Guid userId, Guid newsId)
    {
        return await _newsLikeRepository.DeleteAsync(userId, newsId);
    }
}
