namespace FnBook.Application.DTOs;

public record CreateUfDto(string Sigla, string Nome);

public record UpdateUfDto(string Sigla, string Nome);

public record UfResponseDto(Guid UfId, string Sigla, string Nome);
