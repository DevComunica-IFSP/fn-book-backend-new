using FnBook.Application.DTOs;
using FnBook.Application.Interfaces;
using FnBook.Application.Mappings;
using FnBook.Domain.Interfaces;

namespace FnBook.Application.Services;

public class NewsOriginService : INewsOriginService
{
    private readonly INewsOriginRepository _originRepository;
    private readonly INewsRepository _newsRepository;
    private readonly IUserRepository _userRepository;

    public NewsOriginService(INewsOriginRepository originRepository, INewsRepository newsRepository, IUserRepository userRepository)
    {
        _originRepository = originRepository;
        _newsRepository = newsRepository;
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<NewsOriginResponseDto>> GetAllAsync() =>
        (await _originRepository.GetAllAsync()).Select(o => o.ToResponseDto());

    public async Task<NewsOriginResponseDto?> GetByIdAsync(Guid id) =>
        (await _originRepository.GetByIdAsync(id))?.ToResponseDto();

    public async Task<IEnumerable<NewsOriginResponseDto>> GetByNewsIdAsync(Guid newsId) =>
        (await _originRepository.GetByNewsIdAsync(newsId)).Select(o => o.ToResponseDto());

    public async Task<NewsOriginResponseDto> CreateAsync(CreateNewsOriginDto dto)
    {
        await ValidateReferencesAsync(dto.UserId, dto.NewsId, dto.UfId, dto.Date);
        return (await _originRepository.CreateAsync(dto.ToEntity())).ToResponseDto();
    }

    public async Task<NewsOriginResponseDto?> UpdateAsync(Guid id, UpdateNewsOriginDto dto)
    {
        var origin = await _originRepository.GetByIdAsync(id);
        if (origin is null)
            return null;

        await ValidateReferencesAsync(dto.UserId, dto.NewsId, dto.UfId, dto.Date);
        origin.ApplyUpdate(dto);
        return (await _originRepository.UpdateAsync(origin)).ToResponseDto();
    }

    public Task<bool> DeleteAsync(Guid id) => _originRepository.DeleteAsync(id);

    private async Task ValidateReferencesAsync(Guid userId, Guid newsId, Guid ufId, DateTime date)
    {
        if (userId == Guid.Empty || newsId == Guid.Empty || ufId == Guid.Empty || date == default)
            throw new ArgumentException("Usuário, notícia, UF e data são obrigatórios.");
        if (await _userRepository.GetByIdAsync(userId) is null)
            throw new ArgumentException("Usuário não encontrado.");
        if (await _newsRepository.GetByIdAsync(newsId) is null)
            throw new ArgumentException("Notícia não encontrada.");
    }
}
