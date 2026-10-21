using FnBook.Application.DTOs;
using FnBook.Domain.Entities;

namespace FnBook.Application.Mappings;

public static class ConsultedSourceMapping
{
    public static ConsultedSourceResponseDto ToResponseDto(this ConsultedSource entity) => new()
    {
        Id = entity.Id,
        NewsId = entity.NewsId,
        Url = entity.Url,
        Summary = entity.Summary,
    };

    public static ConsultedSource ToEntity(this CreateConsultedSourceDto dto) => new()
    {
        Id = Guid.NewGuid(),
        NewsId = dto.NewsId,
        Url = dto.Url,
        Summary = dto.Summary,
    };

    public static void ApplyUpdate(this ConsultedSource entity, UpdateConsultedSourceDto dto)
    {
        entity.NewsId = dto.NewsId;
        entity.Url = dto.Url;
        entity.Summary = dto.Summary;
    }
}
