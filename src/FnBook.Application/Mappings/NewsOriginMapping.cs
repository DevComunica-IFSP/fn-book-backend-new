using FnBook.Application.DTOs;
using FnBook.Domain.Entities;

namespace FnBook.Application.Mappings;

public static class NewsOriginMapping
{
    public static NewsOriginResponseDto ToResponseDto(this NewsOrigin origin) =>
        new(origin.OrigemId, origin.UserId, origin.NewsId, origin.UfId, origin.Date);

    public static NewsOrigin ToEntity(this CreateNewsOriginDto dto) => new()
    {
        OrigemId = Guid.NewGuid(),
        UserId = dto.UserId,
        NewsId = dto.NewsId,
        UfId = dto.UfId,
        Date = dto.Date
    };

    public static void ApplyUpdate(this NewsOrigin origin, UpdateNewsOriginDto dto)
    {
        origin.UserId = dto.UserId;
        origin.NewsId = dto.NewsId;
        origin.UfId = dto.UfId;
        origin.Date = dto.Date;
    }
}
