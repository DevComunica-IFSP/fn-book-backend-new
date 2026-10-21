namespace FnBook.Application.DTOs;

public record CreateCommentDto
{
    public Guid UserId { get; init; }
    public Guid NewsId { get; init; }
    public Guid? ParentCommentId { get; init; }
    public string Text { get; init; } = string.Empty;
}

public record UpdateCommentDto
{
    public Guid UserId { get; init; }
    public Guid NewsId { get; init; }
    public Guid? ParentCommentId { get; init; }
    public string Text { get; init; } = string.Empty;
}

public record CommentResponseDto
{
    public Guid Id { get; init; }
    public Guid UserId { get; init; }
    public Guid NewsId { get; init; }
    public Guid? ParentCommentId { get; init; }
    public string Text { get; init; } = string.Empty;
    public DateTime Date { get; init; }
}
