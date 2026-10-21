using System.ComponentModel.DataAnnotations;
using FnBook.Application.DTOs;
using FnBook.Application.Interfaces;
using FnBook.Application.Mappings;
using FnBook.Domain.Interfaces;

namespace FnBook.Application.Services;

public class SimilarNewsService(ISimilarNewsRepository repository, INewsRepository newsRepository) : ISimilarNewsService
{
    public async Task<IEnumerable<SimilarNewsResponseDto>> GetByNewsIdAsync(Guid newsId) =>
        (await repository.GetByNewsIdAsync(newsId)).Select(e => e.ToResponseDto());

    public async Task<SimilarNewsResponseDto?> GetByIdAsync(Guid newsId, Guid similarNewsId) =>
        (await repository.GetByIdAsync(newsId, similarNewsId))?.ToResponseDto();

    public async Task<SimilarNewsResponseDto?> CreateAsync(CreateSimilarNewsDto dto)
    {
        Validator.ValidateObject(dto, new ValidationContext(dto), validateAllProperties: true);
        if (dto.NewsId == Guid.Empty || dto.SimilarNewsId == Guid.Empty || dto.NewsId == dto.SimilarNewsId)
            throw new ArgumentException("Informe duas notícias distintas.");
        if (await newsRepository.GetByIdAsync(dto.NewsId) is null ||
            await newsRepository.GetByIdAsync(dto.SimilarNewsId) is null)
            throw new ArgumentException("Notícia não encontrada.");
        if (await repository.GetByIdAsync(dto.NewsId, dto.SimilarNewsId) is not null)
            return null;
        return (await repository.CreateAsync(dto.ToEntity())).ToResponseDto();
    }

    public async Task<SimilarNewsResponseDto?> UpdateAsync(Guid newsId, Guid similarNewsId, UpdateSimilarNewsDto dto)
    {
        var entity = await repository.GetByIdAsync(newsId, similarNewsId);
        if (entity is null) return null;
        Validator.ValidateObject(dto, new ValidationContext(dto), validateAllProperties: true);
        entity.Similarity = dto.Similarity;
        entity.Date = DateTime.UtcNow;
        return (await repository.UpdateAsync(entity)).ToResponseDto();
    }

    public Task<bool> DeleteAsync(Guid newsId, Guid similarNewsId) => repository.DeleteAsync(newsId, similarNewsId);
}
