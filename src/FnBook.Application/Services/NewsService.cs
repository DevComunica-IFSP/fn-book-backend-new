using FnBook.Application.DTOs;
using FnBook.Application.Interfaces;
using FnBook.Application.Mappings;
using FnBook.Domain.Interfaces;

namespace FnBook.Application.Services;

public class NewsService : INewsService
{
    private readonly INewsRepository _newsRepository;

    public NewsService(INewsRepository newsRepository)
    {
        _newsRepository = newsRepository;
    }

    public async Task<IEnumerable<NewsResponseDto>> GetAllAsync()
    {
        var news = await _newsRepository.GetAllAsync();
        return news.Select(n => n.ToResponseDto());
    }

    public async Task<NewsResponseDto?> GetByIdAsync(Guid id)
    {
        var news = await _newsRepository.GetByIdAsync(id);
        return news?.ToResponseDto();
    }

    public async Task<NewsResponseDto> CreateAsync(CreateNewsDto dto)
    {
        var news = dto.ToEntity();
        var created = await _newsRepository.CreateAsync(news);
        return created.ToResponseDto();
    }

    public async Task<NewsResponseDto?> UpdateAsync(Guid id, UpdateNewsDto dto)
    {
        var news = await _newsRepository.GetByIdAsync(id);
        if (news is null)
            return null;

        news.ApplyUpdate(dto);
        var updated = await _newsRepository.UpdateAsync(news);
        return updated.ToResponseDto();
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        return await _newsRepository.DeleteAsync(id);
    }
}
