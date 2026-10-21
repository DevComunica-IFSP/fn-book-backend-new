using System.ComponentModel.DataAnnotations;
using FnBook.Application.DTOs;
using FnBook.Application.Interfaces;
using FnBook.Application.Mappings;
using FnBook.Domain.Interfaces;

namespace FnBook.Application.Services;

public class ConsultedSourceService(IConsultedSourceRepository repository, INewsRepository newsRepository) : IConsultedSourceService
{
    public async Task<IEnumerable<ConsultedSourceResponseDto>> GetAllAsync() =>
        (await repository.GetAllAsync()).Select(e => e.ToResponseDto());

    public async Task<ConsultedSourceResponseDto?> GetByIdAsync(Guid id) =>
        (await repository.GetByIdAsync(id))?.ToResponseDto();

    public async Task<IEnumerable<ConsultedSourceResponseDto>> GetByNewsIdAsync(Guid newsId) =>
        (await repository.GetByNewsIdAsync(newsId)).Select(e => e.ToResponseDto());

    public async Task<ConsultedSourceResponseDto> CreateAsync(CreateConsultedSourceDto dto)
    {
        await ValidateAsync(dto);
        return (await repository.CreateAsync(dto.ToEntity())).ToResponseDto();
    }

    public async Task<ConsultedSourceResponseDto?> UpdateAsync(Guid id, UpdateConsultedSourceDto dto)
    {
        var entity = await repository.GetByIdAsync(id);
        if (entity is null) return null;
        await ValidateAsync(dto);
        entity.ApplyUpdate(dto);
        return (await repository.UpdateAsync(entity)).ToResponseDto();
    }

    public Task<bool> DeleteAsync(Guid id) => repository.DeleteAsync(id);

    private async Task ValidateAsync(CreateConsultedSourceDto dto)
    {
        Validator.ValidateObject(dto, new ValidationContext(dto), validateAllProperties: true);
        if (dto.NewsId == Guid.Empty || await newsRepository.GetByIdAsync(dto.NewsId) is null)
            throw new ArgumentException("Notícia não encontrada.");
        if (!Uri.TryCreate(dto.Url, UriKind.Absolute, out var uri) || (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
            throw new ArgumentException("A URL deve usar HTTP ou HTTPS.");
    }
}
