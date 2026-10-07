using FnBook.Application.DTOs;

namespace FnBook.Application.Interfaces;

public interface INewsOriginService
{
    Task<IEnumerable<NewsOriginResponseDto>> GetAllAsync();
    Task<NewsOriginResponseDto?> GetByIdAsync(Guid id);
    Task<IEnumerable<NewsOriginResponseDto>> GetByNewsIdAsync(Guid newsId);
    Task<NewsOriginResponseDto> CreateAsync(CreateNewsOriginDto dto);
    Task<NewsOriginResponseDto?> UpdateAsync(Guid id, UpdateNewsOriginDto dto);
    Task<bool> DeleteAsync(Guid id);
}
