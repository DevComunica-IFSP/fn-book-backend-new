using System.ComponentModel.DataAnnotations;

namespace FnBook.Application.DTOs;

public record CreateSimilarNewsDto
{
    public Guid NewsId { get; init; }
    public Guid SimilarNewsId { get; init; }
    [Range(typeof(decimal), "0", "1")]
    public decimal Similarity { get; init; }
}

public record UpdateSimilarNewsDto
{
    [Range(typeof(decimal), "0", "1")]
    public decimal Similarity { get; init; }
}

public record SimilarNewsResponseDto
{
    public Guid NewsId { get; init; }
    public Guid SimilarNewsId { get; init; }
    public decimal Similarity { get; init; }
    public DateTime Date { get; init; }
}
