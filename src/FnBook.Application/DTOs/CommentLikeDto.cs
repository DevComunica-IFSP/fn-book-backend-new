namespace FnBook.Application.DTOs;

public record CreateCommentLikeDto
{
    public Guid UserId { get; init; }
    public Guid CommentId { get; init; }
}

public record CommentLikeResponseDto
{
    public Guid UserId { get; init; }
    public Guid CommentId { get; init; }
}
