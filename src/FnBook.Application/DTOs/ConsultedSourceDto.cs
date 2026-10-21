using System.ComponentModel.DataAnnotations;

namespace FnBook.Application.DTOs;

public record CreateConsultedSourceDto
{
    public Guid NewsId { get; init; }
    [Required]
    [Url]
    [StringLength(2048)]
    public string Url { get; init; } = string.Empty;
    [Required]
    public string Summary { get; init; } = string.Empty;
}

public record UpdateConsultedSourceDto : CreateConsultedSourceDto;

public record ConsultedSourceResponseDto
{
    public Guid Id { get; init; }
    public Guid NewsId { get; init; }
    public string Url { get; init; } = string.Empty;
    public string Summary { get; init; } = string.Empty;
}
