using FnBook.Application.DTOs;
using FnBook.Domain.Entities;

namespace FnBook.Application.Mappings;

public static class SimilarNewsMapping
{
    public static SimilarNewsResponseDto ToResponseDto(this SimilarNews entity) => new()
    {
        NewsId = entity.NewsId,
        SimilarNewsId = entity.SimilarNewsId,
        Similarity = entity.Similarity,
        Date = entity.Date
    };

    public static SimilarNews ToEntity(this CreateSimilarNewsDto dto) => new()
    {
        NewsId = dto.NewsId,
        SimilarNewsId = dto.SimilarNewsId,
        Similarity = dto.Similarity,
        Date = DateTime.UtcNow
    };
}
