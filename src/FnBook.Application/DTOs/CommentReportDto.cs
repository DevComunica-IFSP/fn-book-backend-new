using System.ComponentModel.DataAnnotations;

namespace FnBook.Application.DTOs;

public record CreateCommentReportDto
{
    public Guid CommentId { get; init; }
    public Guid UserId { get; init; }
    [Required]
    public string Reason { get; init; } = string.Empty;
}

public record UpdateCommentReportDto : CreateCommentReportDto;

public record CommentReportResponseDto
{
    public Guid Id { get; init; }
    public Guid CommentId { get; init; }
    public Guid UserId { get; init; }
    public string Reason { get; init; } = string.Empty;
    public DateTime Date { get; init; }
}
