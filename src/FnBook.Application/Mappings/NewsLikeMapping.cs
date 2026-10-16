using FnBook.Application.DTOs;
using FnBook.Domain.Entities;

namespace FnBook.Application.Mappings;

public static class NewsLikeMapping
{
    public static NewsLikeResponseDto ToResponseDto(this NewsLike like) => new()
    {
        UserId = like.UserId,
        NewsId = like.NewsId
    };

    public static NewsLike ToEntity(this CreateNewsLikeDto dto) => new()
    {
        UserId = dto.UserId,
        NewsId = dto.NewsId
    };
}
