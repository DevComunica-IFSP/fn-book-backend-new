using FnBook.Application.DTOs;
using FnBook.Application.Interfaces;
using FnBook.Application.Mappings;
using FnBook.Domain.Interfaces;

namespace FnBook.Application.Services;

public class UfService : IUfService
{
    private readonly IUfRepository _repository;

    public UfService(IUfRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<UfResponseDto>> GetAllAsync() =>
        (await _repository.GetAllAsync()).Select(uf => uf.ToResponseDto());

    public async Task<UfResponseDto?> GetByIdAsync(Guid id) =>
        (await _repository.GetByIdAsync(id))?.ToResponseDto();

    public async Task<UfResponseDto> CreateAsync(CreateUfDto dto)
    {
        Validate(dto.Sigla, dto.Nome);
        var sigla = dto.Sigla.Trim().ToUpperInvariant();
        if (await _repository.ExistsBySiglaAsync(sigla))
            throw new InvalidOperationException("Sigla já cadastrada.");

        return (await _repository.CreateAsync(dto.ToEntity())).ToResponseDto();
    }

    public async Task<UfResponseDto?> UpdateAsync(Guid id, UpdateUfDto dto)
    {
        var uf = await _repository.GetByIdAsync(id);
        if (uf is null)
            return null;

        Validate(dto.Sigla, dto.Nome);
        var sigla = dto.Sigla.Trim().ToUpperInvariant();
        if (await _repository.ExistsBySiglaAsync(sigla, id))
            throw new InvalidOperationException("Sigla já cadastrada.");

        uf.ApplyUpdate(dto);
        return (await _repository.UpdateAsync(uf)).ToResponseDto();
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        if (await _repository.GetByIdAsync(id) is null)
            return false;
        if (await _repository.HasOriginsAsync(id))
            throw new InvalidOperationException("UF está vinculada a uma origem.");

        return await _repository.DeleteAsync(id);
    }

    private static void Validate(string? sigla, string? nome)
    {
        var normalized = sigla?.Trim().ToUpperInvariant();
        if (normalized is null || normalized.Length != 2 || normalized.Any(c => c < 'A' || c > 'Z'))
            throw new ArgumentException("Sigla deve conter duas letras.");
        if (string.IsNullOrWhiteSpace(nome) || nome.Trim().Length > 100)
            throw new ArgumentException("Nome é obrigatório e deve ter no máximo 100 caracteres.");
    }
}
