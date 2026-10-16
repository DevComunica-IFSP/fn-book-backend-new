using FnBook.Application.DTOs;
using FnBook.Domain.Entities;

namespace FnBook.Application.Mappings;

public static class UfMapping
{
    public static UfResponseDto ToResponseDto(this Uf uf) =>
        new(uf.UfId, uf.Sigla, uf.Nome);

    public static Uf ToEntity(this CreateUfDto dto) => new()
    {
        UfId = Guid.NewGuid(),
        Sigla = dto.Sigla.Trim().ToUpperInvariant(),
        Nome = dto.Nome.Trim()
    };

    public static void ApplyUpdate(this Uf uf, UpdateUfDto dto)
    {
        uf.Sigla = dto.Sigla.Trim().ToUpperInvariant();
        uf.Nome = dto.Nome.Trim();
    }
}
