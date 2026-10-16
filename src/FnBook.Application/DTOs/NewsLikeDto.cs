namespace FnBook.Application.DTOs;

public record CreateNewsLikeDto
{
    public Guid UserId { get; init; }
    public Guid NewsId { get; init; }
}

public record NewsLikeResponseDto
{
    public Guid UserId { get; init; }
    public Guid NewsId { get; init; }
}
